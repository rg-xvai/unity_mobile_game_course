using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class Aggro : MonoBehaviour
  {
    public Follow Follow;
    public TriggerObserver TriggerObserver;
    public float Cooldown;
    private Coroutine _aggroCoroutine;
    private bool _hasAggroTarget;

    private void Start()
    {
      TriggerObserver.TriggerEnter += TriggerEnter;
      TriggerObserver.TriggerExit += TriggerExit;
      SwitchFollowOff();
    }

    private void TriggerEnter(Collider obj)
    {
      if (!_hasAggroTarget)
      {
        _hasAggroTarget = true;
        StopAggroCoroutine();
        SwitchFollowOn();
      }
    }

    private void TriggerExit(Collider obj)
    {
      if (_hasAggroTarget)
      {
        _hasAggroTarget = false;
        _aggroCoroutine = StartCoroutine(SwichFollowoffAfterCooldown());
      }
    }

    private void StopAggroCoroutine()
    {
      if (_aggroCoroutine != null)
      {
        StopCoroutine(_aggroCoroutine);
        _aggroCoroutine = null;
      }
    }

    private IEnumerator SwichFollowoffAfterCooldown()
    {
      yield return new WaitForSeconds(Cooldown);
      SwitchFollowOff();
    }

    private void SwitchFollowOn() =>
      Follow.enabled = true;

    private void SwitchFollowOff() =>
      Follow.enabled = false;
  }
}