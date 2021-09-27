using System;
using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
  [Serializable]
  public class ProductConfigWrapper
  {
    public List<ProductConfig> Configs;
  }
}