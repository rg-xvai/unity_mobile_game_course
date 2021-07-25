using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
  [RequireComponent(typeof(EnemyAnimator))]
  public class Attack : MonoBehaviour
  {
    public EnemyAnimator Animator;

    public float AttackCooldown = 3f;
    public float Cleavage = 0.5f;
    public float EffectiveDistance = 0.5f;

    private IGameFactory _factory;
    private Transform _heroTransform;
    private float _attackCooldown;
    private bool _isAttacking;
    private int _layerMask;
    private Collider[] _hits = new Collider[1];


    private void Awake()
    {
      _factory = AllServices.Container.Single<IGameFactory>();
      _factory.HeroCreated += OnHeroCreated;

      _layerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
      UpdateCooldown();

      if (CanAttack())
        StartAttack();
    }

    private void OnAttack()
    {
      if (Hit(out Collider hit))
      {
        
      }
    }

    private void OnAttackEnded()
    {
      _attackCooldown = AttackCooldown;
      _isAttacking = false;
    }

    private bool Hit(out Collider hit)
    {
      int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

      hit = _hits.FirstOrDefault();
      
      return hitsCount > 0;
    }

    private void StartAttack()
    {
      transform.LookAt(_heroTransform);
      Animator.PlayAttack();

      _isAttacking = true;
    }

    private Vector3 StartPoint()
    {
      return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.position * EffectiveDistance;
    }

    private void UpdateCooldown()
    {
      if (!CooldownIsUp())
        _attackCooldown -= Time.deltaTime;
    }

    private void OnHeroCreated() =>
      _heroTransform = _factory.HeroGameObject.transform;

    private bool CanAttack() =>
      !_isAttacking && CooldownIsUp();

    private bool CooldownIsUp() =>
      _attackCooldown <= 0;
  }
}