using UnityEngine;
using UnityEngine.UI;

public class CartItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text productNameText;
    [SerializeField] private Text priceText;
    [SerializeField] private Image productImage;
    [SerializeField] private Button removeButton;
    [SerializeField] private GameObject selectedIndicator;

    private ProductInfo productInfo;
    private int itemIndex;
    private bool isSelected = false;

    public void SetupCartItem(ProductInfo product, int index)
    {
        productInfo = product;
        itemIndex = index;

        if (productNameText) productNameText.text = product.ProductName;
        if (priceText) priceText.text = $"S/ {product.Price:F2}";
        if (productImage) productImage.sprite = product.ProductImage;

        if (removeButton) removeButton.onClick.AddListener(RemoveFromCart);
        if (selectedIndicator) selectedIndicator.SetActive(false);
                
        Button itemButton = GetComponent<Button>();
        if (itemButton == null)
        {
            itemButton = gameObject.AddComponent<Button>();
        }
        itemButton.onClick.AddListener(ToggleSelection);
    }

    private void ToggleSelection()
    {
        isSelected = !isSelected;
        if (selectedIndicator) selectedIndicator.SetActive(isSelected);
                
        CartUI cartUI = Object.FindFirstObjectByType<CartUI>();
        if (cartUI != null)
        {
            if (isSelected)
                cartUI.SelectItem(this);
            else
                cartUI.DeselectItem(this);
        }
    }

    private void RemoveFromCart()
    {
        CartManager.Instance.RemoveFromCart(itemIndex);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        if (selectedIndicator) selectedIndicator.SetActive(isSelected);
    }

    public ProductInfo GetProductInfo()
    {
        return productInfo;
    }

    public int GetItemIndex()
    {
        return itemIndex;
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}