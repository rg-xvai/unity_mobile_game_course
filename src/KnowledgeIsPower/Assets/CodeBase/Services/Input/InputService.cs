using UnityEngine;

namespace CodeBase.Services.Input
{
  public abstract class InputService : IInputService
  {
    protected const string Horizontal = "Horizontal";
    protected const string Vertical = "Vertical";
    private const string FireButton = "Fire";
    
    public abstract Vector2 Axis { get; }

    protected Vector2 SimpleInputAxis => 
      new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));

    public bool IsAttackButtonUp() => 
      SimpleInput.GetButtonUp(FireButton);
  }
}