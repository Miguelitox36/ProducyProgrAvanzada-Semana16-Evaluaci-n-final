using UnityEngine;

public interface IPaymentStrategy
{
    float ProcessPayment(float amount);
    string GetPaymentMethod();
}