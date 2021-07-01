using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure
{
  public class Game
  {
    public static IInputService InputService;
    public GameStateMachine StateMachine;

    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
    {
      StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain);
    }
  }
}