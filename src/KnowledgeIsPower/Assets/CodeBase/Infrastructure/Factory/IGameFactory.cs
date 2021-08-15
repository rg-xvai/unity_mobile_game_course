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
    GameObject CreateHero(GameObject at);
    GameObject CreateHud();
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    void Cleanup();
    void Register(ISavedProgressReader progressReader);
    GameObject CreateMonster(MonsterTypeId typeId, Transform parent);
    LootPiece CreateLoot();
  }
}