using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.PresistenProgress;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    private readonly IAssets _assets;
    private readonly IStaticDataService _staticData;
    private readonly IRandomService _randomService;
    private readonly IPersistentProgressService _progressService;
    private readonly IWindowService _windowService;

    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
    private GameObject HeroGameObject { get; set; }

    public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService, IWindowService windowService)
    {
      _assets = assets;
      _staticData = staticData;
      _randomService = randomService;
      _progressService = progressService;
      _windowService = windowService;
    }

    public async Task WarmUp()
    {
      await _assets.Load<GameObject>(AssetAddress.Loot);
      await _assets.Load<GameObject>(AssetAddress.Spawner);
    }
    public async Task<GameObject> CreateHero(Vector3 at)
    {
      HeroGameObject = await InstantiateRegisteredAsync(AssetAddress.Hero, at);
      return HeroGameObject;
    }

    public async Task<GameObject> CreateHud()
    {
      GameObject hud = await InstantiateRegistered(AssetAddress.Hud);
      hud.GetComponentInChildren<LootCounter>()
        .Construct(_progressService.Progress.WorldData);
      foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>()) 
        openWindowButton.Construct(_windowService);
      return hud;
    }

    public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
    {
      MonsterStaticData monsterData = _staticData.ForMonster(typeId);

      GameObject prefab = await _assets.Load<GameObject>(monsterData.PrefabReference);
      GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
      IHealth health = monster.GetComponent<IHealth>();
      health.Current = monsterData.Hp;
      health.Max = monsterData.Hp;

      monster.GetComponent<ActorUI>().Construct(health);
      monster.GetComponent<AgentMoveToPlayer>().Construct(HeroGameObject.transform);
      monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

      LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
      lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
      lootSpawner.Construct(this, _randomService);

      Attack attack = monster.GetComponent<Attack>();
      attack.Construct(HeroGameObject.transform);
      attack.Damage = monsterData.Damage;
      attack.Cleavage = monsterData.Cleavage;
      attack.EffectiveDistance = monsterData.EffectiveDistance;

      monster.GetComponent<RotateToPlayer>()?.Construct(HeroGameObject.transform);

      return monster;
    }

    public async Task<LootPiece> CreateLoot(Vector3 at)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Loot);
      LootPiece lootPiece = InstantiateRegisteredAsync(prefab, at)
        .GetComponent<LootPiece>();

      lootPiece.Construct(_progressService.Progress.WorldData);

      return lootPiece;
    }

    public async Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);
      SpawnPoint spawner = InstantiateRegisteredAsync(prefab, at)
        .GetComponent<SpawnPoint>();

      spawner.Construct(this);
      spawner.Id = spawnerId;
      spawner.MonsterTypeId = monsterTypeId;
    }

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
      _assets.CleanUp();
    }

    private void Register(ISavedProgressReader progressReader)
    {
      if (progressReader is ISavedProgress progressWriter)
        ProgressWriters.Add(progressWriter);

      ProgressReaders.Add(progressReader);
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
    {
      GameObject gameObject = await _assets.Instantiate(prefabPath, at);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }
    private GameObject InstantiateRegisteredAsync(GameObject prefab, Vector3 at)
    {
      GameObject gameObject = Object.Instantiate(prefab, at,Quaternion.identity);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }
    private async Task<GameObject> InstantiateRegistered(string prefabPath)
    {
      GameObject gameObject = await _assets.Instantiate(prefabPath);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
      foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
        Register(progressReader);
    }
  }
}