namespace CashInn.Model.MenuItem;

public class SpecialItem : AbstractMenuItem
{
    public DateTime ValidFrom
    {
        get => _validFrom;
        set
        {
            // if (value > DateTime.Now)
                // throw new ArgumentException("ValidFrom cannot be in the future", nameof(ValidFrom));
            _validFrom = value;
        }
    }
    private DateTime _validFrom;
    
    public DateTime ValidTo
    {
        get => _validTo;
        set
        {
            if (value < ValidFrom)
                throw new ArgumentException("ValidTo cannot be before ValidFrom", nameof(ValidTo));
            _validTo = value;
        }
    }
    private DateTime _validTo;
    public override string ItemType => "Special";
    public SpecialItem(int id, string name, double price, string description, string dietaryInformation, bool available,
        DateTime validFrom, DateTime validTo)
        : base(id, name, price, description, dietaryInformation, available)
    {
        ValidFrom = validFrom;
        ValidTo = validTo;
        
        SaveItem(this);
    }
    
    public override object ToSerializableObject()
    {
        return new
        {
            Id,
            Name,
            Price,
            Description,
            DietaryInformation,
            Available,
            ValidFrom,
            ValidTo,
            ItemType
        };
    }
}