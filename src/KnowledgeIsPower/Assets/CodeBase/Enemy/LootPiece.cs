using System;
using System.Collections;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class LootPiece : MonoBehaviour, ISavedProgress
  {
    public GameObject Skull;
    public GameObject PickupFxPrefab;
    public TextMeshPro LootText;
    public GameObject PickupPopup;

    private Loot _loot;
    private bool _picked;
    private WorldData _worldData;
    private SpawnedLoot _spawnedLoot;

    public void Construct(WorldData worldData) =>
      _worldData = worldData;

    public void Initialize(Loot loot)
    {
      _loot = loot;
      _spawnedLoot = CreateSpawnedLoot();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      if (_spawnedLoot != null && !progress.WorldData.SpawnedItems.Exists(x => x.Id == _spawnedLoot.Id))
      {
        progress.WorldData.SpawnedItems.Add(_spawnedLoot);
      }
    }

    public void LoadProgress(PlayerProgress progress)
    {
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

    private SpawnedLoot CreateSpawnedLoot() =>
      new SpawnedLoot
      {
        Loot = _loot,
        PositionOnLevel = new PositionOnLevel(GetCurrentLevel(), transform.position.AsVectorData()),
        Id = GenerateId()
      };

    private void RemoveFromList()
    {
      _worldData.SpawnedItems?.RemoveAll(x => x.Id == _spawnedLoot.Id);
      _spawnedLoot = null;
    }
    
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

    private string GenerateId() =>
      Guid.NewGuid().ToString();

    private string GetCurrentLevel() =>
      gameObject.scene.name;
  }
}