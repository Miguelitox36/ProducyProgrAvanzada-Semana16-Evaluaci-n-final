using UnityEngine;
using UnityEngine.UI;
using GameJolt.API;

public class ThankYouScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button newPurchaseButton;
    [SerializeField] private Button quitGameButton;
    [SerializeField] private Text thankYouText;
    [SerializeField] private Text purchaseSummaryText;

    private void Start()
    {
        SetupButtons();
        UpdatePurchaseSummary();
    }

    private void SetupButtons()
    {
        if (newPurchaseButton)
        {
            newPurchaseButton.onClick.AddListener(StartNewPurchase);
        }

        if (quitGameButton)
        {
            quitGameButton.onClick.AddListener(QuitGame);
        }
    }

    private void UpdatePurchaseSummary()
    {
        if (purchaseSummaryText && CartManager.Instance != null)
        {
            float finalTotal = CartManager.Instance.GetFinalTotal();
            string paymentMethod = CartManager.Instance.GetCurrentPaymentMethod();
            purchaseSummaryText.text = $"Total pagado: S/ {finalTotal:F2}\nMétodo: {paymentMethod}";
        }
    }

    private void StartNewPurchase()
    {        
        if (CartManager.Instance != null)
        {
            CartManager.Instance.ClearCart();
        }
        
        if (ScreenManager.Instance != null)
        {
            ScreenManager.Instance.ShowScreen(ScreenManager.ScreenState.ProductDetection);
        }
    }

    private void QuitGame()
    {        
        if (thankYouText)
        {
            thankYouText.text = "¡Gracias por visitar PlazaVea!";
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}