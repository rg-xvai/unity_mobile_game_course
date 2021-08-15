using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
  class UnityRandomService : IRandomService
  {
    public int Next(int min, int max) => 
      Random.Range(min, max);
  }
}