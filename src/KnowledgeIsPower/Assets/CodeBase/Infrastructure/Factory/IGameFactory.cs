using System.Collections.Generic;
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
    
    GameObject CreateHero(Vector3 at);
    GameObject CreateHud();
    GameObject CreateMonster(MonsterTypeId typeId, Transform parent);
    LootPiece CreateLoot(Vector3 at);
    void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
    
    void Cleanup();
  }
}