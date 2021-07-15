using System;
using UnityEngine;

namespace CodeBase.Infrastructure
{
  public class GameRunner : MonoBehaviour
  {
    public GameBootstrapper BootstrapperPrefab;

    private void Awake()
    {
      var bootstrapper = GameObject.FindObjectOfType<GameBootstrapper>();
      if (bootstrapper == null)
        Instantiate(BootstrapperPrefab);
    }
  }
}