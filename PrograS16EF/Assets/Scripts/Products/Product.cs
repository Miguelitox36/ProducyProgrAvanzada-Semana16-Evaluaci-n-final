using UnityEngine;

[System.Serializable]
public abstract class Product : ScriptableObject  
{
    [SerializeField] protected string productName;
    [SerializeField] protected float price;
    [SerializeField] protected Sprite productImage;
    [SerializeField] protected string category;

    public abstract float GetDiscountedPrice();
    public abstract string GetProductDescription();

    public string ProductName => productName;
    public float Price => price;
    public Sprite ProductImage => productImage;
    public string Category => category;
   
    public virtual void SetProductData(string name, float basePrice, Sprite image, string cat)
    {
        productName = name;
        price = basePrice;
        productImage = image;
        category = cat;
    }
}