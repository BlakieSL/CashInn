namespace CashInn.Model.Payment;

[Serializable]
public class CardPayment : AbstractPayment
{
    public override string PaymentType => "Card";
    public string CardNumber
    {
        get => _cardNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("CardNumber cannot be null or empty", nameof(CardNumber));
            _cardNumber = value;
        }
    }

    private string _cardNumber;
    public CardPayment(int id, double amount, DateTime dateOfPayment, string cardNumber) 
        : base(id, amount, dateOfPayment)
    {
        CardNumber = cardNumber;
        
        SavePayment(this);
    }

    public override object ToSerializableObject()
    {
        return new
        {
            Id,
            Amount,
            CardNumber,
            DateOfPayment,
            PaymentType
        };
    }
}