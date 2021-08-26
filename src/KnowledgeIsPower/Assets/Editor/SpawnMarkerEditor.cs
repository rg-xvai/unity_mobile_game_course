using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;

namespace Editor
{
  [CustomEditor(typeof(SpawnMarker))]
  public class SpawnMarkerEditor : UnityEditor.Editor
  {
    private static float monsterScale = 0.005f;
    private static Dictionary<MonsterTypeId, MonsterStaticData> _monsters;

    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(SpawnMarker spawner, GizmoType gizmo)
    {
      Color before = Gizmos.color;
      
      Gizmos.color = new Color(0.25f, 1f, 1f, 0.8f);

      GUIStyle textStyle = new GUIStyle();
      textStyle.normal.textColor = Color.magenta;
      textStyle.fontStyle = FontStyle.BoldAndItalic;
      Handles.Label(spawner.transform.position.AddY(-0.2f), $"{spawner.MonsterTypeId}", textStyle);
      
      if (_monsters == null)
        LoadMonstersData();
      Mesh mesh = _monsters[spawner.MonsterTypeId].Prefab.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
      Gizmos.DrawMesh(mesh, 0, spawner.transform.position, Quaternion.identity, new Vector3(monsterScale, monsterScale, monsterScale));

      Gizmos.color = before;
    }

    private static void LoadMonstersData()
    {
      _monsters = Resources
        .LoadAll<MonsterStaticData>("StaticData/Monsters")
        .ToDictionary(x => x.MonsterTypeId, x => x);
    }
  }
}