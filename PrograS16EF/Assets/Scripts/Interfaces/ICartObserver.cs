using UnityEngine;
using System.Collections.Generic;

public interface ICartObserver 
{
    void OnCartUpdated(List<ProductInfo> cart, float total);
}
