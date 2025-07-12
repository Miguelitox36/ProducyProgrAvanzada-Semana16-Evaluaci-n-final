using UnityEngine;
using Vuforia;

public class ProductScanner : MonoBehaviour
{
    [Header("Tipo de Producto")]
    [SerializeField] private ProductType productType = ProductType.Food;

    [Header("Información Básica")]
    [SerializeField] private string productName;
    [SerializeField] private float price;
    [SerializeField] private Sprite productImage;
    [SerializeField] private string category;
    [SerializeField] private int stock;

    [Header("Configuración de Alimentos")]
    [SerializeField] private string expirationDate = "2024-12-31";
    [SerializeField] private bool isOrganic = false;

    [Header("Configuración de Electrónicos")]
    [SerializeField] private int warrantyMonths = 12;
    [SerializeField] private string brand = "Generic";

    [Header("Estado del Scanner")]
    [SerializeField] private bool isProductDetected = false;

    private ProductInfo productData;
    private Product specificProduct;
    private ObserverBehaviour observerBehaviour;

    void Start()
    {        
        observerBehaviour = GetComponent<ObserverBehaviour>();
        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        CreateProductData();
    }
        
    void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED ||
            targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            OnProductDetected();
        }
        else if (targetStatus.Status == Status.NO_POSE)
        {
            OnProductLost();
        }
    }

    private void CreateProductData()
    {
        try
        {
            productData = new ProductInfo(productName, price, productImage, category, stock);

            switch (productType)
            {
                case ProductType.Food:
                    specificProduct = CreateFoodProduct();
                    break;
                case ProductType.Electronics:
                    specificProduct = CreateElectronicsProduct();
                    break;
            }

            Debug.Log($"Producto {productType} configurado: {productName} - S/ {price:F2}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al crear producto: {e.Message}");
        }
    }

    private FoodProduct CreateFoodProduct()
    {
        var foodProduct = ScriptableObject.CreateInstance<FoodProduct>();
        foodProduct.SetFoodData(productName, price, productImage, category, expirationDate, isOrganic);
        return foodProduct;
    }

    private ElectronicsProduct CreateElectronicsProduct()
    {
        var electronicsProduct = ScriptableObject.CreateInstance<ElectronicsProduct>();
        electronicsProduct.SetElectronicsData(productName, price, productImage, category, warrantyMonths, brand);
        return electronicsProduct;
    }

    public void OnProductDetected()
    {
        try
        {
            
            if (ScreenManager.Instance == null || !ScreenManager.Instance.CanAddToCart())
            {
                return;
            }

            isProductDetected = true;
            if (UIManager.Instance != null)
            {
                float discountedPrice = specificProduct.GetDiscountedPrice();
                ProductInfo discountedProductInfo = new ProductInfo(
                    productName,
                    discountedPrice,
                    productImage,
                    category,
                    stock
                );

                UIManager.Instance.ShowProduct(discountedProductInfo);

                Debug.Log($"Producto detectado: {productName}");
                Debug.Log($"Precio original: S/ {price:F2}");
                Debug.Log($"Precio con descuento: S/ {discountedPrice:F2}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al detectar producto: {e.Message}");
        }
    }

    public void OnProductLost()
    {
        try
        {
            isProductDetected = false;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.HideProduct();
                Debug.Log("Producto perdido");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al perder producto: {e.Message}");
        }
    }

    void OnDestroy()
    {
        if (observerBehaviour != null)
        {
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }

        if (specificProduct != null)
        {
            DestroyImmediate(specificProduct);
        }
    }
    
    public ProductInfo GetCurrentProduct() => productData;
    public Product GetSpecificProduct() => specificProduct;
    public ProductType GetProductType() => productType;
    public bool IsProductConfigured() => productData != null && specificProduct != null;
}

public enum ProductType
{
    Food,
    Electronics
}