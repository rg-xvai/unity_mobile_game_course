using System;
using CodeBase.Cameralogic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure
{
  public class LoadLevelState: IPayloadedState<string>
  {
    private const string InitialPointTag = "InitialPoint";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;

    public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
    }

    public void Enter(string sceneName) => 
      _sceneLoader.Load(sceneName,OnLoaded);

    public void Exit()
    {
    }

    private void OnLoaded()
    { 
      GameObject initialPoint = GameObject.FindWithTag(InitialPointTag);
      GameObject hero = Instantiate("Hero/hero",at: initialPoint.transform.position);
     Instantiate("Hud/Hud");
     CameraFollow(hero);
    }

    private void CameraFollow(GameObject hero)
    {
      Camera.main
        .GetComponent<CameraFollow>()
        .Follow(hero);
    }

    private static GameObject Instantiate(string path)
    {
      var prefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(prefab);
    }
    private static GameObject Instantiate(string path, Vector3 at)
    {
      var prefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(prefab,at,Quaternion.identity);
    }
  }
}