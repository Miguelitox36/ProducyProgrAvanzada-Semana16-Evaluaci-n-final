using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartUI : MonoBehaviour, ICartObserver
{
    [Header("Cart UI Elements")]
    [SerializeField] private Transform cartItemsContainer;
    [SerializeField] private GameObject cartItemPrefab;
    [SerializeField] private Text totalText;
    [SerializeField] private Text itemCountText;
    [SerializeField] private Button removeSelectedButton;
    [SerializeField] private Button clearCartButton;
    [SerializeField] private Text emptyCartMessage;

    private List<CartItemUI> cartItemUIs = new List<CartItemUI>();
    private List<CartItemUI> selectedItems = new List<CartItemUI>();

    private void Start()
    {
        CartManager.Instance.AddObserver(this);
        SetupButtons();
        UpdateCartDisplay();
    }

    private void SetupButtons()
    {
        if (removeSelectedButton)
        {
            removeSelectedButton.onClick.AddListener(RemoveSelectedItems);
            removeSelectedButton.gameObject.SetActive(false);
        }

        if (clearCartButton) clearCartButton.onClick.AddListener(ClearCart);
    }

    public void OnCartUpdated(List<ProductInfo> cart, float total)
    {
        UpdateCartDisplay();
    }

    private void UpdateCartDisplay()
    {       
        ClearCartItemUIs();

        var cart = CartManager.Instance.GetCart();
                
        if (emptyCartMessage) emptyCartMessage.gameObject.SetActive(cart.Count == 0);
                
        for (int i = 0; i < cart.Count; i++)
        {
            CreateCartItemUI(cart[i], i);
        }

        if (totalText) totalText.text = $"Total: S/ {CartManager.Instance.GetTotal():F2}";
        if (itemCountText) itemCountText.text = $"Productos: {cart.Count}";
                
        UpdateRemoveSelectedButton();
    }

    private void CreateCartItemUI(ProductInfo product, int index)
    {
        if (cartItemPrefab == null || cartItemsContainer == null) return;

        GameObject itemObject = Instantiate(cartItemPrefab, cartItemsContainer);
        CartItemUI cartItemUI = itemObject.GetComponent<CartItemUI>();

        if (cartItemUI == null)
        {
            cartItemUI = itemObject.AddComponent<CartItemUI>();
        }

        cartItemUI.SetupCartItem(product, index);
        cartItemUIs.Add(cartItemUI);
    }

    private void ClearCartItemUIs()
    {
        foreach (var itemUI in cartItemUIs)
        {
            if (itemUI != null && itemUI.gameObject != null)
            {
                Destroy(itemUI.gameObject);
            }
        }
        cartItemUIs.Clear();
        selectedItems.Clear();
    }

    public void SelectItem(CartItemUI item)
    {
        if (!selectedItems.Contains(item))
        {
            selectedItems.Add(item);
            UpdateRemoveSelectedButton();
        }
    }

    public void DeselectItem(CartItemUI item)
    {
        selectedItems.Remove(item);
        UpdateRemoveSelectedButton();
    }

    private void UpdateRemoveSelectedButton()
    {
        if (removeSelectedButton)
        {
            bool hasSelection = selectedItems.Count > 0;
            removeSelectedButton.gameObject.SetActive(hasSelection);

            if (hasSelection)
            {
                removeSelectedButton.GetComponentInChildren<Text>().text =
                    $"Eliminar Seleccionados ({selectedItems.Count})";
            }
        }
    }

    private void RemoveSelectedItems()
    {       
        selectedItems.Sort((a, b) => b.GetItemIndex().CompareTo(a.GetItemIndex()));

        foreach (var item in selectedItems)
        {
            CartManager.Instance.RemoveFromCart(item.GetItemIndex());
        }

        selectedItems.Clear();
        UpdateRemoveSelectedButton();
    }

    private void ClearCart()
    {
        CartManager.Instance.ClearCart();
        selectedItems.Clear();
        UpdateRemoveSelectedButton();
    }

    private void OnDestroy()
    {
        if (CartManager.Instance != null)
        {
            CartManager.Instance.RemoveObserver(this);
        }
    }
}