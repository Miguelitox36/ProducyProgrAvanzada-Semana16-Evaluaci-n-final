using UnityEngine;

[System.Serializable]
public class ProductInfoEditor
{
    [Header("Información del Producto")]
    [SerializeField] private string productName;
    [SerializeField] private float price;
    [SerializeField] private Sprite productImage;
    [SerializeField] private string category;
    [SerializeField] private int stock;

    [Header("Información Adicional")]
    [TextArea(2, 4)]
    [SerializeField] private string description;
        
    public ProductInfo ToProductInfo()
    {
        return new ProductInfo(productName, price, productImage, category, stock);
    }
       
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(productName) &&
               price > 0 &&
               !string.IsNullOrEmpty(category) &&
               stock >= 0;
    }
        
    public string GetProductName() => productName;
    public float GetPrice() => price;
    public string GetCategory() => category;
    public int GetStock() => stock;
}