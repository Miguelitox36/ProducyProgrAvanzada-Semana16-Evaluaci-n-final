using UnityEngine;

public class DebitCardPaymentStrategy : IPaymentStrategy
{
    public float ProcessPayment(float amount)
    {
        return amount * 0.98f; // 2% de descuento
    }

    public string GetPaymentMethod()
    {
        return "Tarjeta de Débito";
    }
}