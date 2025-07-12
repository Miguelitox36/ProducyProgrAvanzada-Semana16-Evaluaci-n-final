using GameJolt.API;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour, ICartObserver
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject productPanel;
    [SerializeField] private Text nameText;
    [SerializeField] private Text priceText;
    [SerializeField] private Image productImage;
    [SerializeField] private Button addToCartButton;

    [Header("Cart UI")]
    [SerializeField] private Text cartListText;
    [SerializeField] private Text totalText;
    [SerializeField] private Text finalTotalText;
    [SerializeField] private Text paymentMethodText;

    [Header("Navigation Buttons")]
    [SerializeField] private Button continueShoppingButton;
    [SerializeField] private Button proceedToPaymentButton;
    [SerializeField] private Button viewCartButton;
    [SerializeField] private Button finishPurchaseButton;

    [Header("Payment Buttons")]
    [SerializeField] private Button cashButton;
    [SerializeField] private Button creditCardButton;
    [SerializeField] private Button debitCardButton;
    [SerializeField] private Button undoButton;

    private ProductInfo currentProduct;
    private bool scoreSubmitted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetupButtons();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CartManager.Instance.AddObserver(this);

        if (productPanel) productPanel.SetActive(false);
    }

    private void SetupButtons()
    {
        if (addToCartButton) addToCartButton.onClick.AddListener(OnAddToCart);


        if (continueShoppingButton) continueShoppingButton.onClick.AddListener(OnContinueShopping);
        if (proceedToPaymentButton) proceedToPaymentButton.onClick.AddListener(() => ScreenManager.Instance.ShowScreen(ScreenManager.ScreenState.Payment));
        if (viewCartButton) viewCartButton.onClick.AddListener(OnViewCart);
        if (finishPurchaseButton) finishPurchaseButton.onClick.AddListener(OnFinishPurchase);


        if (cashButton) cashButton.onClick.AddListener(() => SetPaymentMethod(new CashPaymentStrategy()));
        if (creditCardButton) creditCardButton.onClick.AddListener(() => SetPaymentMethod(new CreditCardPaymentStrategy()));
        if (debitCardButton) debitCardButton.onClick.AddListener(() => SetPaymentMethod(new DebitCardPaymentStrategy()));
        if (undoButton) undoButton.onClick.AddListener(() => CartManager.Instance.UndoLastAction());
    }

    private void OnContinueShopping()
    {
        
        Trophies.Unlock(273729);
        ScreenManager.Instance.ShowScreen(ScreenManager.ScreenState.ProductDetection);
    }

    private void OnViewCart()
    {
        
        Trophies.Unlock(273726);
        ScreenManager.Instance.ShowScreen(ScreenManager.ScreenState.ProductDetection);
    }

    private void OnCashPayment()
    {
        
        Trophies.Unlock(273727);

        SetPaymentMethod(new CashPaymentStrategy());
    }

    private void OnCreditCardPayment()
    {
        Trophies.Unlock(273728);
        SetPaymentMethod(new CreditCardPaymentStrategy());
    }

    private void SetPaymentMethod(IPaymentStrategy strategy)
    {
        CartManager.Instance.SetPaymentStrategy(strategy);
        if (paymentMethodText) paymentMethodText.text = $"Método: {strategy.GetPaymentMethod()}";
        if (finalTotalText) finalTotalText.text = $"Total Final: S/ {CartManager.Instance.GetFinalTotal():F2}";
    }

    public void ShowProduct(ProductInfo product)
    {
        try
        {
            currentProduct = product;
            if (nameText) nameText.text = product.ProductName;
            if (priceText) priceText.text = $"S/ {product.Price:F2}";
            if (productImage) productImage.sprite = product.ProductImage;
            if (productPanel) productPanel.SetActive(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al mostrar producto: {e.Message}");
        }
    }

    public void HideProduct()
    {
        if (productPanel) productPanel.SetActive(false);
        currentProduct = null;
    }

    public void OnAddToCart()
    {
        if (currentProduct != null)
        {
            CartManager.Instance.AddToCart(currentProduct);
            Debug.Log($"Producto agregado al carrito: {currentProduct.ProductName}");
        }
    }

    public void OnCartUpdated(List<ProductInfo> cart, float total)
    {
        try
        {
            
            if (cartListText)
            {
                cartListText.text = "";
                foreach (var item in cart)
                    cartListText.text += $"{item.ProductName} - S/ {item.Price:F2}\n";
            }

            
            if (totalText) totalText.text = $"Subtotal: S/ {total:F2}";

            
            if (finalTotalText) finalTotalText.text = $"Total Final: S/ {CartManager.Instance.GetFinalTotal():F2}";
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al actualizar UI: {e.Message}");
        }
    }

    public void OnFinishPurchase()
    {
        try
        {
            Trophies.Unlock(273730);

            float finalTotal = CartManager.Instance.GetFinalTotal();

            int scoreValue = Mathf.CeilToInt(finalTotal * 100);
            string scoreText = $"S/ {finalTotal:F2} soles";
                       
            Scores.Add(scoreValue, scoreText, 1028254, " ", (success) => {
                scoreSubmitted = true;
                Debug.Log($"Score enviado: {scoreValue} - {scoreText} - Éxito: {success}");
            });

            if (cartListText) cartListText.text = "¡Compra finalizada!";
            if (totalText) totalText.text = $"Total pagado: S/ {finalTotal:F2}";
            ScreenManager.Instance.ShowScreen(ScreenManager.ScreenState.ProductDetection);
            CartManager.Instance.ClearCart();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al finalizar compra: {e.Message}");
            
        }
    }

    private void OnDestroy()
    {
        if (CartManager.Instance != null)
            CartManager.Instance.RemoveObserver(this);
    }
}