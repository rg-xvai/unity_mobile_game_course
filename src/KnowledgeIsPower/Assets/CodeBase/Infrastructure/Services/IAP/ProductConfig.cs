using System;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
  [Serializable]
  public class ProductConfig
  {
    public string Id;
    public ProductType Type;

    public int MaxPurchaseCount;
  }
}