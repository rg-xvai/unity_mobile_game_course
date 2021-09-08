using System.Collections.Generic;
using System.Linq;
using CodeBase.Cameralogic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.PresistenProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private const string InitialPointTag = "InitialPoint";
    private const string EnemySpawnerTag = "EnemySpawner";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private string _sceneName;

    public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain,
      IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _curtain = curtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
      _staticData = staticData;
    }

    public void Enter(string sceneName)
    {
      _sceneName = sceneName;
      _curtain.Show();
      _gameFactory.Cleanup();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
      _curtain.Hide();

    private void OnLoaded()
    {
      InitGameWorld();
      InformProgressReaders();

      _stateMachine.Enter<GameLoopState>();
    }

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.Progress);
    }

    private void InitGameWorld()
    {
      InitSpawners();
      InitLootPieces();

      GameObject hero = _gameFactory.CreateHero(at: GameObject.FindWithTag(InitialPointTag));

      InitHud(hero);

      CameraFollow(hero);
    }

    private void InitSpawners()
    {
      string sceneKey = SceneManager.GetActiveScene().name;
      LevelStaticData levelData = _staticData.ForLevel(sceneKey);
      foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
      {
        _gameFactory.CreateSpawner(spawnerData.position, spawnerData.Id, spawnerData.MonsterTypeId);
      }
    }

    private void InitLootPieces()
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
        LootPiece lootPiece = _gameFactory.CreateLoot(spawnedItem.PositionOnLevel.Position.AsUnityVector());
        lootPiece.Initialize(spawnedItem.Loot);
      }
    }

    private void InitHud(GameObject hero)
    {
      GameObject hud = _gameFactory.CreateHud();

      hud.GetComponentInChildren<ActorUI>()
        .Construct(hero.GetComponent<HeroHealth>());
    }

    private void CameraFollow(GameObject hero)
    {
      Camera.main
        .GetComponent<CameraFollow>()
        .Follow(hero);
    }
  }
}