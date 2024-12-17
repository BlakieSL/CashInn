using CashInn.Model.MenuItem;

namespace CashInn.Model;

public class AbstractMenuItemOrderAssociation
{
    public AbstractMenuItem MenuItem { get; private set; }
    public Order Order { get; private set; }
    
    private int _quantity;
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("quantity cannot be negative", nameof(value));
            }
            _quantity = value;
        }
    }

    public AbstractMenuItemOrderAssociation(AbstractMenuItem menuItem, Order order, int quantity)
    {
        Quantity = quantity;
        MenuItem = menuItem;
        Order = order;
    }

    public void Update(int quantity)
    {
        Quantity = quantity;
    }
}