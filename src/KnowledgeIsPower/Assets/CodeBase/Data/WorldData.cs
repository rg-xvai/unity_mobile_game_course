using System;
using System.Collections.Generic;
using CodeBase.Enemy;

namespace CodeBase.Data
{
  [Serializable]
  public class WorldData
  {
    public PositionOnLevel PositionOnLevel;
    public LootData LootData;
    public List<SpawnedLoot> SpawnedItems = new List<SpawnedLoot>();

    public WorldData(string initialLevel)
    {
      PositionOnLevel = new PositionOnLevel(initialLevel);
      LootData = new LootData();
    }
  }
}