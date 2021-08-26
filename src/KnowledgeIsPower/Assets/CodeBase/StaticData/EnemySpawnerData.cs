using System;
using UnityEngine;

namespace CodeBase.StaticData
{
  [Serializable]
  public class EnemySpawnerData
  {
    public string Id;
    public MonsterTypeId MonsterTypeId;
    public Vector3 position;

    public EnemySpawnerData(string id, MonsterTypeId monsterTypeId, Vector3 position)
    {
      Id = id;
      MonsterTypeId = monsterTypeId;
      this.position = position;
    }
  }
}