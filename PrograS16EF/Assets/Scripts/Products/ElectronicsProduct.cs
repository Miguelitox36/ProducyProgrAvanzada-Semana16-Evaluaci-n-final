using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Electronics Product", menuName = "Products/Electronics Product")]
public class ElectronicsProduct : Product
{
    [SerializeField] private int warrantyMonths;
    [SerializeField] private string brand;

    public override float GetDiscountedPrice()
    {
        return warrantyMonths > 12 ? price * 0.90f : price * 0.95f;
    }

    public override string GetProductDescription()
    {
        return $"Electrónico {brand} - Garantía: {warrantyMonths} meses";
    }
    
   
    public void SetElectronicsData(string name, float basePrice, Sprite image, string cat, int warranty, string brandName)
    {
        SetProductData(name, basePrice, image, cat);
        warrantyMonths = warranty;
        brand = brandName;
    }
    
    public int WarrantyMonths => warrantyMonths;
    public string Brand => brand;
}