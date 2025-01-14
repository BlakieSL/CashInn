namespace CashInn.Model.Payment;

[Serializable]
public class CardPayment : IPaymentRole
{
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

    public string PaymentType => "Card";

    public CardPayment(string cardNumber)
    {
        CardNumber = cardNumber;
    }

    public object GetDetails()
    {
        return new { CardNumber };
    }
}