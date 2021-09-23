using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    
    Task<GameObject> CreateHero(Vector3 at);
    Task<GameObject> CreateHud();
    Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent);
    Task<LootPiece> CreateLoot(Vector3 at);
    Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
    
    void Cleanup();
    Task WarmUp();
  }
}