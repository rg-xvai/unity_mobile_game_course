using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class RotateToPlayer : Follow
  {
    public float Speed;
    private const float MinimalDistance = 1;
    private Transform _heroTransform;
    private IGameFactory _gameFactory;
    private Vector3 _positionLook;

    private void Start()
    {
      _gameFactory = AllServices.Container.Single<IGameFactory>();
      if (_gameFactory.HeroGameObject != null)
        InitialHeroTransform();
      else
        _gameFactory.HeroCreated += HeroCreated;
    }

    private void Update()
    {
      if (Initialized())
        RotateTowardsHero();
    }

    private void InitialHeroTransform() =>
      _heroTransform = _gameFactory.HeroGameObject.transform;

    private bool Initialized() =>
      _heroTransform != null;

    private void HeroCreated() =>
      InitialHeroTransform();

    private void RotateTowardsHero()
    {
      UpdatePositionToLookAt();
      transform.rotation = SmoothedRotation(transform.rotation, _positionLook);
    }

    private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionLook) =>
      Quaternion.Lerp(rotation, TargetRotation(positionLook), SpeedFactor());

    private Quaternion TargetRotation(Vector3 position) =>
      Quaternion.LookRotation(position);

    private float SpeedFactor() =>
      Speed * Time.deltaTime;

    private void UpdatePositionToLookAt()
    {
      Vector3 positionDiff = _heroTransform.position - transform.position;
      _positionLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
    }
  }
}