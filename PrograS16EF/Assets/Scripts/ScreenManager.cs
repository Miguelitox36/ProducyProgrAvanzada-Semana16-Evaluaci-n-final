using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [Header("Pantallas")]
    [SerializeField] private GameObject productDetectionScreen;    
    [SerializeField] private GameObject cartReviewScreen;         
    [SerializeField] private GameObject paymentScreen;           
    [SerializeField] private GameObject thankYouScreen;           

    [Header("Botones de Navegación")]
    [SerializeField] private Button viewCartButton;               
    [SerializeField] private Button continueShoppingButton;      
    [SerializeField] private Button proceedToPaymentButton;       
    [SerializeField] private Button backToCartButton;            
    [SerializeField] private Button finishPurchaseButton;         
    [SerializeField] private Button newPurchaseButton;            

    [Header("Indicadores")]
    [SerializeField] private Text cartItemCountText;              
    [SerializeField] private GameObject hasItemsIndicator;       

    public enum ScreenState
    {
        ProductDetection,
        CartReview,
        Payment,
        ThankYou
    }

    private ScreenState currentScreen = ScreenState.ProductDetection;

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
        ShowScreen(ScreenState.ProductDetection);
        UpdateCartIndicator();
    }

    private void SetupButtons()
    {
        if (viewCartButton) viewCartButton.onClick.AddListener(() => ShowScreen(ScreenState.CartReview));
        if (continueShoppingButton) continueShoppingButton.onClick.AddListener(() => ShowScreen(ScreenState.ProductDetection));
        if (proceedToPaymentButton) proceedToPaymentButton.onClick.AddListener(() => ShowScreen(ScreenState.Payment));
        if (backToCartButton) backToCartButton.onClick.AddListener(() => ShowScreen(ScreenState.CartReview));
        if (finishPurchaseButton) finishPurchaseButton.onClick.AddListener(FinishPurchase);
        if (newPurchaseButton) newPurchaseButton.onClick.AddListener(() => ShowScreen(ScreenState.ProductDetection));
    }

    public void ShowScreen(ScreenState screen)
    {
        
        if (productDetectionScreen) productDetectionScreen.SetActive(false);
        if (cartReviewScreen) cartReviewScreen.SetActive(false);
        if (paymentScreen) paymentScreen.SetActive(false);
        if (thankYouScreen) thankYouScreen.SetActive(false);

        
        switch (screen)
        {
            case ScreenState.ProductDetection:
                if (productDetectionScreen) productDetectionScreen.SetActive(true);
                break;
            case ScreenState.CartReview:
                if (cartReviewScreen) cartReviewScreen.SetActive(true);
                break;
            case ScreenState.Payment:
                if (paymentScreen) paymentScreen.SetActive(true);
                break;
            case ScreenState.ThankYou:
                if (thankYouScreen) thankYouScreen.SetActive(true);
                break;
        }

        currentScreen = screen;
        UpdateScreenButtons();
    }

    private void UpdateScreenButtons()
    {        
        bool hasItems = CartManager.Instance.GetCart().Count > 0;

        if (viewCartButton) viewCartButton.gameObject.SetActive(currentScreen == ScreenState.ProductDetection && hasItems);
        if (continueShoppingButton) continueShoppingButton.gameObject.SetActive(currentScreen == ScreenState.CartReview);
        if (proceedToPaymentButton) proceedToPaymentButton.gameObject.SetActive(currentScreen == ScreenState.CartReview && hasItems);
        if (backToCartButton) backToCartButton.gameObject.SetActive(currentScreen == ScreenState.Payment);
        if (finishPurchaseButton) finishPurchaseButton.gameObject.SetActive(currentScreen == ScreenState.Payment && hasItems);
        if (newPurchaseButton) newPurchaseButton.gameObject.SetActive(currentScreen == ScreenState.ThankYou);
    }

    public void UpdateCartIndicator()
    {
        int itemCount = CartManager.Instance.GetCart().Count;

        if (cartItemCountText) cartItemCountText.text = itemCount.ToString();
        if (hasItemsIndicator) hasItemsIndicator.SetActive(itemCount > 0);

        UpdateScreenButtons();
    }

    private void FinishPurchase()
    {        
        float total = CartManager.Instance.GetFinalTotal();
        Debug.Log($"Compra finalizada. Total: S/ {total:F2}");
                
        CartManager.Instance.ClearCart();
                
        ShowScreen(ScreenState.ThankYou);
                
        UpdateCartIndicator();
               
        StartCoroutine(ReturnToMainScreenAfterDelay(3f));
    }

    private IEnumerator ReturnToMainScreenAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowScreen(ScreenState.ProductDetection);
    }

    public ScreenState GetCurrentScreen()
    {
        return currentScreen;
    }

    public bool CanAddToCart()
    {
        return currentScreen == ScreenState.ProductDetection;
    }
}