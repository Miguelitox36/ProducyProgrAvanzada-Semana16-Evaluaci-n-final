using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Food Product", menuName = "Products/Food Product")]
public class FoodProduct : Product
{
    [SerializeField] private string expirationDate;
    [SerializeField] private bool isOrganic;

    public override float GetDiscountedPrice()
    {
        return isOrganic ? price * 0.95f : price * 0.98f;
    }

    public override string GetProductDescription()
    {
        return $"Alimento {(isOrganic ? "orgánico" : "convencional")} - Vence: {expirationDate}";
    }
       
    public void SetFoodData(string name, float basePrice, Sprite image, string cat, string expDate, bool organic)
    {
        SetProductData(name, basePrice, image, cat);
        expirationDate = expDate;
        isOrganic = organic;
    }

    public string ExpirationDate => expirationDate;
    public bool IsOrganic => isOrganic;
}