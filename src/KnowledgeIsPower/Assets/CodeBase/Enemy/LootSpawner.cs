using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class LootSpawner : MonoBehaviour
  {
    public EnemyDeath EnemyDeath;
    private IGameFactory _factory;
    private IRandomService _random;
    
    private int _lootMin;
    private int _lootMax;

    public void Construct(IGameFactory factory, IRandomService random)
    {
      _factory = factory;
      _random = random;
    }
    private void Start()
    {
      EnemyDeath.Happened += SpawnLoot;
    }

    private void SpawnLoot()
    {
      GameObject loot = _factory.CreateLoot();
      loot.transform.position = transform.position;

      Loot lootItem = new Loot()
      {
        Value = _random.Next(_lootMin, _lootMax)
      };
    }

    public void SetLoot(int min, int max)
    {
      _lootMin = min;
      _lootMax = max;
    }
  }
}