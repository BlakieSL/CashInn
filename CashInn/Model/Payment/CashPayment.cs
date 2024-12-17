namespace CashInn.Model.Payment;

[Serializable]
public class CashPayment : AbstractPayment
{
    public override string PaymentType => "Cash";

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
    public CashPayment(int id, double amount, double cashReceived, double changeGiven, DateTime dateOfPayment, Order? order = null) 
        : base(id, amount, dateOfPayment, order)
    {
        CashReceived = cashReceived;
        ChangeGiven = changeGiven;
        
        SavePayment(this);
    }
    
    public override object ToSerializableObject()
    {
        return new
        {
            Id,
            Amount,
            CashReceived,
            ChangeGiven,
            DateOfPayment,
            PaymentType
        };
    }
}