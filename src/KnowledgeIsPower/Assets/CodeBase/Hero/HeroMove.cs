using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
  public class HeroMove : MonoBehaviour, ISavedProgress
  {
    [SerializeField] private float _movementSpeed;
    [SerializeField] private CharacterController _characterController;

    private IInputService _inputService;

    private Camera _camera;

    public void UpdateProgress(PlayerProgress progress)
    {
      progress.WorldData.Position = transform.position.AsVectorData();
    }

    public void LoadProgress(PlayerProgress progress)
    {
      throw new System.NotImplementedException();
    }

    private void Awake()
    {
      _camera = Camera.main;
      _inputService = AllServices.Container.Single<IInputService>();
    }

    private void Update()
    {
      Vector3 movementVector = Vector3.zero;

      if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
      {
        movementVector = _camera.transform.TransformDirection(_inputService.Axis);
        movementVector.y = 0;
        movementVector.Normalize();

        transform.forward = movementVector;
      }

      movementVector += Physics.gravity;

      _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);
    }
  }
}