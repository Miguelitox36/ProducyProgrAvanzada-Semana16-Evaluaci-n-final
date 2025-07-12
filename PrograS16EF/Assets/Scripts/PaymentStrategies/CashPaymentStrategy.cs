using UnityEngine;

public class CashPaymentStrategy : IPaymentStrategy
{
    public float ProcessPayment(float amount)
    {
        return amount;
    }

    public string GetPaymentMethod()
    {
        return "Efectivo";
    }
}