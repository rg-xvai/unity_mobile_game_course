using System;
using CodeBase.Hero;
using UnityEngine;

namespace CodeBase.UI
{
  public class ActorUI : MonoBehaviour
  {
    public HpBar HpBar;

    private HeroHealth _heroHealth;

    private void OnDestroy() => 
      _heroHealth.HealthChanged -= UpdateHpBar;

    public void Construct(HeroHealth health)
    {
      _heroHealth = health;

      _heroHealth.HealthChanged += UpdateHpBar;
    }

    private void UpdateHpBar()
    {
      HpBar.SetValue(_heroHealth.Current, _heroHealth.Max);
    }
  }
}