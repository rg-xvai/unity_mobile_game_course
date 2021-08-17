using System;
using System.Collections;
using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Enemy
{
  public class LootPiece : MonoBehaviour, ISavedProgress
  {
    public GameObject Skull;
    public GameObject PickupFxPrefab;
    public TextMeshPro LootText;
    public GameObject PickupPopup;
    public UniqueId UniqueId;
    
    private Loot _loot;
    private bool _picked;
    private WorldData _worldData;
    

    public void Construct(WorldData worldData) =>
      _worldData = worldData;

    public void LoadProgress(PlayerProgress progress)
    {
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      if (!gameObject.activeSelf)
        return;
      
      SpawnedLoot spawnedLoot = new SpawnedLoot
      {
        Loot = _loot,
        PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData()),
        Id = UniqueId.Id
      };
      
      if (!progress.WorldData.SpawnedItems.Exists(x => x.Id == spawnedLoot.Id))
      {
        progress.WorldData.SpawnedItems.Add(spawnedLoot);
      }
    }

    public void Initialize(Loot loot)
    {
      _loot = loot;
    }

    private void OnTriggerEnter(Collider other) => PickUp();

    private void PickUp()
    {
      if (_picked)
        return;

      _picked = true;

      UpdateWorldData();
      HideSkull();
      PlayPickupFx();
      ShowText();
      RemoveFromList();
      StartCoroutine(StartDestroyTimer());
    }

    private void RemoveFromList() =>
      _worldData.SpawnedItems?.RemoveAll(x => x.Id == UniqueId.Id);

    private void UpdateWorldData() =>
      _worldData.LootData.Collect(_loot);

    private void HideSkull() =>
      Skull.SetActive(false);

    private IEnumerator StartDestroyTimer()
    {
      yield return new WaitForSeconds(1.5f);
      Destroy(gameObject);
    }

    private void PlayPickupFx() =>
      Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);

    private void ShowText()
    {
      LootText.text = $"{_loot.Value}";
      PickupPopup.SetActive(true);
    }

    private static string CurrentLevel()
    {
      return SceneManager.GetActiveScene().name;
    }
  }
}