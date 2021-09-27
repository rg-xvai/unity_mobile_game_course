using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.ResourceManagement.Util;

namespace CodeBase.Infrastructure.Services.IAP
{
  public class IAPProvider : IStoreListener
  {
    private const string IAPCOnfigPath = "IAP/products";
    private IStoreController _controller;
    private IExtensionProvider _extensions;
    private IAPService _iapService;

    public Dictionary<string,ProductConfig> Configs { get; set; }

    public event Action Initialized;
    public bool IsInitialized => _controller != null && _extensions != null;

    public void Initialize(IAPService iapService)
    {
      _iapService = iapService;
      Configs = new Dictionary<string, ProductConfig>();
      
      Load();

      ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
     
      foreach (ProductConfig productConfig in Configs.Values) 
        builder.AddProduct(productConfig.Id, productConfig.ProductType);
      
      UnityPurchasing.Initialize( this, builder);
    }

    public void StartPurchase(string productId) => 
      _controller.InitiatePurchase(productId);

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
      _controller = controller;
      _extensions = extensions;

      Initialized?.Invoke();
      Debug.Log("UnityPurchasing initialization success");
    }

    public void OnInitializeFailed(InitializationFailureReason error) => 
      Debug.Log($"UnityPurchasing OnInitializeFailed:{error}");

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
      Debug.Log($"UnityPurchasing ProcessPurchase success{purchaseEvent.purchasedProduct.definition.id}");
      return _iapService.ProcessingPurchase(purchaseEvent.purchasedProduct);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) => 
      Debug.LogError($"Product{product.definition.id} purchase failed, PurchaseFailureReason {failureReason}, transaction id {product.transactionID}");

    private void Load() => 
      Configs = Resources
        .Load<TextAsset>(IAPCOnfigPath)
        .text
        .ToDeserialized<ProductConfigWrapper>()
        .Configs
        .ToDictionary(x => x.Id, x=>x);
  }
}