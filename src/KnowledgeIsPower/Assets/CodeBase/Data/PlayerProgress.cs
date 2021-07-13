using System;
using UnityEngine;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {
    private readonly string _initialLevel;

    public PlayerProgress(string initialLevel)
    {
      _initialLevel = initialLevel;
    }

    public WorldData WorldData;
  }
}