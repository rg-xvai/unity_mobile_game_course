using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PresistenProgress;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
  public class IAPService : IIAPService
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

    public List<ProductDescription> Products() => 
      ProductDescriptions().ToList();

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

    private IEnumerable<ProductDescription> ProductDescriptions()
    {
      PurchaseDate purchaseDate = _progressService.Progress.PurchaseDate;
      foreach (string productsId in _iapProvider.Products.Keys)
      {
        ProductConfig config = _iapProvider.Configs[productsId];
        Product product = _iapProvider.Products[productsId];
        BoughIAP boughIAP = purchaseDate.BoughIAPs.Find(x=>x.IApid == productsId);
        if(ProductBoughtOut(boughIAP, config))
          continue;
        yield return new ProductDescription()
        {
          Id = productsId,
          Config = config,
          Product = product,
          AvailablePurchasesLeft = boughIAP != null
            ? config.MaxPurchaseCount - boughIAP.Count
            : config.MaxPurchaseCount,
        };
      }
    }

    private static bool ProductBoughtOut(BoughIAP boughIAP, ProductConfig config) => 
      boughIAP!=null&& boughIAP.Count >= config.MaxPurchaseCount;
  }
}