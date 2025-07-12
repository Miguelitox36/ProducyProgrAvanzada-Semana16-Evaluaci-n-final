using UnityEngine;

[System.Serializable]
public class ProductInfo
{
    [SerializeField] private string productName;
    [SerializeField] private float price;
    [SerializeField] private Sprite productImage;
    [SerializeField] private string category;
    [SerializeField] private int stock;

    public ProductInfo(string name, float price, Sprite image, string category, int stock)
    {
        this.productName = name;
        this.price = price;
        this.productImage = image;
        this.category = category;
        this.stock = stock;
    }

    // Getters públicos necesarios
    public string ProductName => productName;
    public float Price => price;
    public Sprite ProductImage => productImage;
    public string Category => category;
    public int Stock => stock;
}
