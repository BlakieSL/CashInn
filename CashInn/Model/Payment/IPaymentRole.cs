namespace CashInn.Model.Payment;

public interface IPaymentRole
{
    string PaymentType { get; }
    object GetDetails();
}