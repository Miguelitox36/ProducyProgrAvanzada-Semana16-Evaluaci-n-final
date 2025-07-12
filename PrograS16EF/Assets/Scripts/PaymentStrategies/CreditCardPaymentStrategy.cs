using UnityEngine;

public class CreditCardPaymentStrategy : IPaymentStrategy
{
    public float ProcessPayment(float amount)
    {
        return amount * 1.03f; // 3% de recargo
    }

    public string GetPaymentMethod()
    {
        return "Tarjeta de Crédito";
    }
}