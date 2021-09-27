using System;
using CodeBase.Infrastructure.Services.PresistenProgress;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
  public class IAPService
  {
    private readonly IAPProvider _iapProvider;
    private readonly IPersistentProgressService _progressService;
    public bool IsInitialized => _iapProvider.IsInitialized;
    public event Action Initialized;

    public IAPService(IAPProvider iapProvider, IPersistentProgressService progressService)
    {
      _iapProvider = iapProvider;
      _progressService = progressService;
    }

    public void Initialize()
    {
      _iapProvider.Initialize(this);
      _iapProvider.Initialized += () => Initialized?.Invoke();
    }

    public void StartPurchase(string productId) => 
      _iapProvider.StartPurchase(productId);

    public PurchaseProcessingResult ProcessingPurchase(Product purchasedProduct)
    {
      ProductConfig productConfig = _iapProvider.Configs[purchasedProduct.definition.id];
      switch (productConfig.ItemType)
      {
        case ItemType.Skulls:
          _progressService.Progress.WorldData.LootData.Add(productConfig.Quantity);
          _progressService.Progress.PurchaseDate.AddPurchase(purchasedProduct.definition.id);
          break;
      }

      return PurchaseProcessingResult.Complete;
    }
  }
}