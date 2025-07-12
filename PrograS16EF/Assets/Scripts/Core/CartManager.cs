using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CartManager : MonoBehaviour
{
    public static CartManager Instance { get; private set; }

    private List<ProductInfo> cart = new List<ProductInfo>();
    private Stack<string> actionHistory = new Stack<string>();
    private Queue<ProductInfo> processingQueue = new Queue<ProductInfo>();
    private Dictionary<string, int> productCount = new Dictionary<string, int>();
    private List<ICartObserver> observers = new List<ICartObserver>();
    private IPaymentStrategy paymentStrategy;

    private GenericDataManager<FoodProduct> foodManager = new GenericDataManager<FoodProduct>();
    private GenericDataManager<ElectronicsProduct> electronicsManager = new GenericDataManager<ElectronicsProduct>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePaymentStrategy();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePaymentStrategy()
    {
        paymentStrategy = new CashPaymentStrategy();
    }

    public void AddObserver(ICartObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void RemoveObserver(ICartObserver observer)
    {
        observers.Remove(observer);
    }

    private void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.OnCartUpdated(cart, GetTotal());
        }
    }

    public void AddToCart(ProductInfo product)
    {
        try
        {
            cart.Add(product);
            actionHistory.Push($"Added {product.ProductName}");
            processingQueue.Enqueue(product);

            if (productCount.ContainsKey(product.ProductName))
                productCount[product.ProductName]++;
            else
                productCount[product.ProductName] = 1;

            NotifyObservers();

            // Actualizar indicadores de navegación
            if (ScreenManager.Instance != null)
            {
                ScreenManager.Instance.UpdateCartIndicator();
            }

            Debug.Log($"Producto agregado: {product.ProductName}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al agregar producto: {e.Message}");
        }
    }

    public void RemoveFromCart(int index)
    {
        try
        {
            if (index >= 0 && index < cart.Count)
            {
                ProductInfo product = cart[index];
                cart.RemoveAt(index);

                // Actualizar conteo
                if (productCount.ContainsKey(product.ProductName))
                {
                    productCount[product.ProductName]--;
                    if (productCount[product.ProductName] <= 0)
                        productCount.Remove(product.ProductName);
                }

                actionHistory.Push($"Removed {product.ProductName}");
                NotifyObservers();

                // Actualizar indicadores de navegación
                if (ScreenManager.Instance != null)
                {
                    ScreenManager.Instance.UpdateCartIndicator();
                }

                Debug.Log($"Producto eliminado: {product.ProductName}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al remover producto: {e.Message}");
        }
    }

    public void UndoLastAction()
    {
        try
        {
            if (actionHistory.Count > 0 && cart.Count > 0)
            {
                string lastAction = actionHistory.Pop();
                ProductInfo lastProduct = cart.Last();
                cart.RemoveAt(cart.Count - 1);

                if (productCount.ContainsKey(lastProduct.ProductName))
                {
                    productCount[lastProduct.ProductName]--;
                    if (productCount[lastProduct.ProductName] <= 0)
                        productCount.Remove(lastProduct.ProductName);
                }

                NotifyObservers();

                if (ScreenManager.Instance != null)
                {
                    ScreenManager.Instance.UpdateCartIndicator();
                }

                Debug.Log($"Acción deshecha: {lastAction}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al deshacer acción: {e.Message}");
        }
    }

    public void ClearCart()
    {
        cart.Clear();
        actionHistory.Clear();
        processingQueue.Clear();
        productCount.Clear();
        NotifyObservers();

        if (ScreenManager.Instance != null)
        {
            ScreenManager.Instance.UpdateCartIndicator();
        }
    }

    // Métodos de acceso
    public float GetTotal()
    {
        return cart.Sum(product => product.Price);
    }

    public float GetFinalTotal()
    {
        return paymentStrategy.ProcessPayment(GetTotal());
    }

    public void SetPaymentStrategy(IPaymentStrategy strategy)
    {
        paymentStrategy = strategy;
    }

    public List<ProductInfo> GetCart() => cart;
    public Dictionary<string, int> GetProductCount() => productCount;
    public int GetQueueCount() => processingQueue.Count;
    public string GetLastAction() => actionHistory.Count > 0 ? actionHistory.Peek() : "No hay acciones";
    public string GetCurrentPaymentMethod() => paymentStrategy.GetPaymentMethod();
}