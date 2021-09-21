using System;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Logic
{
  public class LevelTransferTrigger : MonoBehaviour
  {
    private const string PlayerTag = "Player";
    public string TransferTo;
    private IGameStateMachine _stateMachine;
    private bool _triggered;
    private void Awake()
    {
      _stateMachine = AllServices.Container.Single<IGameStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
      if(_triggered) 
        return;
      if (other.CompareTag(PlayerTag))
      {
        _stateMachine.Enter<LoadLevelState, string>(TransferTo);
        _triggered = true;
      }
    }
  }
}