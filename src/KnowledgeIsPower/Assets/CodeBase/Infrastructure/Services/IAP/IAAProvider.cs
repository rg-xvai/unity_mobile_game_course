using System;
using System.Collections.Generic;
using CodeBase.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.ResourceManagement.Util;

namespace CodeBase.Infrastructure.Services.IAP
{
  public class IAAProvider : IStoreListener
  {
    private const string IAPCOnfigPath = "IAP/products";
    private IStoreController _controller;
    private IExtensionProvider _extensions;
    
    private List<ProductConfig> _configs;

    public event Action Initialized;
    public bool IsInitialized => _controller != null && _extensions != null;

    public void Initialize()
    {
      Load();

      ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
     
      foreach (ProductConfig productConfig in _configs) 
        builder.AddProduct(productConfig.Id, productConfig.Type);
      
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
      return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) => 
      Debug.LogError($"Product{product.definition.id} purchase failed, PurchaseFailureReason {failureReason}, transaction id {product.transactionID}");

    private void Load() => 
      _configs = Resources.Load<TextAsset>(IAPCOnfigPath).text.ToDeserialized<ProductConfigWrapper>().Configs;
  }
}