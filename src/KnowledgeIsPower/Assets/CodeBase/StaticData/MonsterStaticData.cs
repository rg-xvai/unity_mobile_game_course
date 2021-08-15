using UnityEngine;

namespace CodeBase.StaticData
{
  [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
  public class MonsterStaticData : ScriptableObject
  {
    public MonsterTypeId MonsterTypeId;

    [Range(1, 100)] public int Hp;

    [Range(1f, 30f)] public float Damage;

    public int MinLoot;
    public int MaxLoot;

    [Range(0.5f, 1f)] public float EffectiveDistance = 0.666f;

    [Range(0.5f, 1f)] public float Cleavage;

    public GameObject Prefab;

    [Range(0.2f, 5f)] public float MoveSpeed = 0.5f;
  }
}