﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.PresistenProgress;
using UnityEngine;

namespace CodeBase.UI.Windows
{
  public class ShopItemsContainer : MonoBehaviour
  {
    private const string ShopItemPath = "ShopItem";

    public GameObject[] ShopUnavailableObjects;
    public Transform Parent;
    private IIAPService _iapService;
    private IPersistentProgressService _progressService;
    private IAssets _assets;
    private List<GameObject> _shopItems;

    public void Construct(IIAPService iapService, IPersistentProgressService progressService, IAssets assets)
    {
      _iapService = iapService;
      _progressService = progressService;
      _assets = assets;
    }

    public void Initialize() =>
      RefreshAvailableItems();

    public void Subscribe()
    {
      _iapService.Initialized += RefreshAvailableItems;
      _progressService.Progress.PurchaseDate.Changed += RefreshAvailableItems;
    }

    public void Cleanup()
    {
      _iapService.Initialized -= RefreshAvailableItems;
      _progressService.Progress.PurchaseDate.Changed -= RefreshAvailableItems;
    }

    private async void RefreshAvailableItems()
    {
      UpdateShopUnavailableObjects();
      if (_iapService.IsInitialized)
        return;

      foreach (GameObject shopItem in _shopItems)
        Destroy(shopItem);

      foreach (ProductDescription productDescription in _iapService.Products())
      {
        GameObject shopItemObject = await _assets.Instantiate(ShopItemPath, Parent);
        ShopItem shopItem = shopItemObject.GetComponent<ShopItem>();

        _shopItems.Add(shopItemObject);
      }
    }

    private void UpdateShopUnavailableObjects()
    {
      foreach (GameObject shopUnavailableObject in ShopUnavailableObjects)
        shopUnavailableObject.SetActive(!_iapService.IsInitialized);
    }
  }
}