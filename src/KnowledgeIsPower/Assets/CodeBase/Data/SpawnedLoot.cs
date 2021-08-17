using System;

namespace CodeBase.Data
{
  [Serializable]
  public class SpawnedLoot
  {
    public PositionOnLevel PositionOnLevel;
    public Loot Loot;
    public string Id;
  }
}