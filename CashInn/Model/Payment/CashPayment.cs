namespace CashInn.Model.Payment;

[Serializable]
public class CashPayment : IPaymentRole
{
    public double CashReceived
    {
        get => _cashReceived;
        set
        {
            if (value <= 0)
                throw new ArgumentException("CashReceived cannot be less than 0", nameof(CashReceived));
            _cashReceived = value;
        }
    }
    private double _cashReceived;
    
    public double? ChangeGiven
    {
        get => _changeGiven;
        set
        {
            if (value < 0)
                throw new ArgumentException("ChangeGiven cannot be less than 0", nameof(ChangeGiven));
            _changeGiven = value;
        }
    }
    private double? _changeGiven;

    public string PaymentType => "Cash";
    public object GetDetails()
    {
        return new { CashReceived, ChangeGiven };
    }
}