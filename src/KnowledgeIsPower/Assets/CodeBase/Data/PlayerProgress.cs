using System;
using UnityEngine;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {
    public State HeroState;
    public WorldData WorldData;
    public Stats HeroStats;
    public KillData KillData;
    public PurchaseDate PurchaseDate;


    public PlayerProgress(string initialLevel)
    {
      WorldData = new WorldData(initialLevel);
      HeroState = new State();
      HeroStats = new Stats();
      KillData = new KillData();
      PurchaseDate = new PurchaseDate();
    }
  }
}