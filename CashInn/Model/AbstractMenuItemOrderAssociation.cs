using CashInn.Model.MenuItem;

namespace CashInn.Model;

public class AbstractMenuItemOrderAssociation
{
    public AbstractMenuItem menuItem { get; private set; }
    public Order order { get; private set; }
    
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
        this.menuItem = menuItem;
        this.order = order;
    }

    public void Update(int quantity)
    {
        Quantity = quantity;
    }
}