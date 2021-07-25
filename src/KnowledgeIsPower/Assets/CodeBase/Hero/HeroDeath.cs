using UnityEngine;

namespace CodeBase.Hero
{
  [RequireComponent(typeof(HeroHealth))]
  public class HeroDeath : MonoBehaviour
  {
    public HeroHealth Health;

    public HeroMove Move;
    public HeroAttack Attack;
    public HeroAnimator Animator;

    public GameObject DeathFx;
    private bool _isDeath;

    private void Start() => 
      Health.HealthChanged += HealthChanged;

    private void OnDestroy() => 
      Health.HealthChanged -= HealthChanged;

    private void HealthChanged()
    {
      if (!_isDeath && Health.Current <= 0)
        Die();
    }

    private void Die()
    {
      _isDeath = true;
      
      Move.enabled = false;
      Attack.enabled = false;
      
      Animator.PlayDeath();

      Instantiate(DeathFx, transform.position, Quaternion.identity);
    }
  }
}