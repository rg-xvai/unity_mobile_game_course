using System;
using CodeBase.Hero;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI
{
  public class ActorUI : MonoBehaviour
  {
    public HpBar HpBar;

    private IHealth _heroHealth;

    private void OnDestroy() => 
      _heroHealth.HealthChanged -= UpdateHpBar;

    public void Construct(IHealth health)
    {
      _heroHealth = health;

      _heroHealth.HealthChanged += UpdateHpBar;
    }

    private void Start()
    {
      IHealth health = GetComponent<IHealth>();

      if (health != null)
        Construct(health);
    }

    private void UpdateHpBar()
    {
      HpBar.SetValue(_heroHealth.Current, _heroHealth.Max);
    }
  }
}