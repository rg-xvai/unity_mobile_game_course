using System.Linq;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
  [CustomEditor(typeof(LevelStaticData))]
  public class LevelStaticDataEditor : UnityEditor.Editor
  {
    private const string InitialPointTag = "InitialPoint";
    
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      LevelStaticData levelData = (LevelStaticData)target;

      if (GUILayout.Button("Collect"))
      {
        levelData.EnemySpawners = FindObjectsOfType<SpawnMarker>()
          .Select(x => new EnemySpawnerData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, x.transform.position))
          .ToList();

        levelData.LevelKey = EditorSceneManager.GetActiveScene().name;
        levelData.InitialHeroPosition= GameObject.FindWithTag(InitialPointTag).transform.position;
      }
      
      EditorUtility.SetDirty(target);
    }
  }
}