using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Cameralogic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.PresistenProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private const string EnemySpawnerTag = "EnemySpawner";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IUIFactory _uiFactory;
    private string _sceneName;

    public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain,
      IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData, IUIFactory uiFactory)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _curtain = curtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
      _staticData = staticData;
      _uiFactory = uiFactory;
    }

    public void Enter(string sceneName)
    {
      _sceneName = sceneName;
      _curtain.Show();
      _gameFactory.Cleanup();
      _gameFactory.WarmUp();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
      _curtain.Hide();

    private async void OnLoaded()
    {
      InitUIRoot();
     await InitGameWorld();
      InformProgressReaders();

      _stateMachine.Enter<GameLoopState>();
    }

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.Progress);
    }

    private void InitUIRoot() => 
      _uiFactory.CreateUIRoot();

    private async Task InitGameWorld()
    {
      LevelStaticData levelData = LevelStaticData();

     await InitSpawners(levelData);
     await InitLootPieces();
    
      GameObject hero = InitHero(levelData);

      InitHud(hero);

      CameraFollow(hero);
    }

    private GameObject InitHero(LevelStaticData levelData) => 
      _gameFactory.CreateHero(at: levelData.InitialHeroPosition);

    private async Task InitSpawners(LevelStaticData levelData)
    {

      foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners) 
        await _gameFactory.CreateSpawner(spawnerData.position, spawnerData.Id, spawnerData.MonsterTypeId);
    }

    private async Task InitLootPieces()
    {
      List<SpawnedLoot> spawnedItems = _progressService
        .Progress
        .WorldData
        .SpawnedItems
        .FindAll(w => w.PositionOnLevel.Level == _sceneName)
        .ToList();

      foreach (SpawnedLoot spawnedItem in spawnedItems)
      {
        _progressService.Progress.WorldData.SpawnedItems.RemoveAll(x => x.Id == spawnedItem.Id);
        LootPiece lootPiece = await _gameFactory.CreateLoot(spawnedItem.PositionOnLevel.Position.AsUnityVector());
        lootPiece.Initialize(spawnedItem.Loot);
      }
    }

    private void InitHud(GameObject hero)
    {
      GameObject hud = _gameFactory.CreateHud();

      hud.GetComponentInChildren<ActorUI>()
        .Construct(hero.GetComponent<HeroHealth>());
    }

    private LevelStaticData LevelStaticData() => 
      _staticData.ForLevel(SceneManager.GetActiveScene().name);

    private void CameraFollow(GameObject hero)
    {
      Camera.main
        .GetComponent<CameraFollow>()
        .Follow(hero);
    }
  }
}