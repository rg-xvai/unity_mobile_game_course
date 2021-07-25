﻿using System;
using UnityEngine;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {
    public State HeroState;
    public WorldData WorldData;
    public Stats HeroStats;

    public PlayerProgress(string initialLevel)
    {
      WorldData = new WorldData(initialLevel);
      HeroState = new State();
      HeroStats = new Stats();
    }
  }
}