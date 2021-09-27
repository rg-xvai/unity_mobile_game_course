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
    private List<ProductConfig> _configs;

    public void Initialize()
    {
      ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
      Load();
      
      foreach (ProductConfig productConfig in _configs) 
        builder.AddProduct(productConfig.Id, productConfig.Type);
      
      UnityPurchasing.Initialize( this, builder);
    }

    private void Load()
    {
      _configs = Resources.Load<TextAsset>(IAPCOnfigPath).text.ToDeserialized<ProductConfigWrapper>().Configs;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
      throw new System.NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
      throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
      throw new System.NotImplementedException();
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
      throw new System.NotImplementedException();
    }
  }
}