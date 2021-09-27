using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
  [Serializable]
  public class PurchaseDate
  {
    public List<BoughIAP> BoughIAPs = new List<BoughIAP>();
    public Action Changed;
    public void AddPurchase(string id)
    {
      BoughIAP boughIaPs = Product(id);
      
      if (boughIaPs != null)
        boughIaPs.Count++;
      else
        BoughIAPs.Add(new BoughIAP( {IApid = id, Count = 1 });
      
      Changed?.Invoke();
    }

    private BoughIAP Product(string id) => 
      BoughIAPs.Find(x => x.IApid == id);
  }
}