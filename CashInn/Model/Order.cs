using CashInn.Helper;
using CashInn.Model.Employee;
using CashInn.Model.MenuItem;
using CashInn.Model.Payment;

namespace CashInn.Model;

public class Order : ClassExtent<Order>
{
    protected override string FilePath => ClassExtentFiles.OrdersFile;

    private readonly List<AbstractMenuItemOrderAssociation> _menuItemAssociations = [];
    public IEnumerable<AbstractMenuItemOrderAssociation> MenuItemAssociations => _menuItemAssociations.AsReadOnly();
    public Customer Customer { get; private set; }
    public DeliveryEmpl? DeliveryEmpl { get; private set; }
    public Payment.Payment Payment { get; private set; }
    public Review? Review { get; private set; }

    public int Id
    {
        get => _id;
        set
        {
            if (value < 0)
                throw new ArgumentException("Id cannot be less than 0", nameof(Id));
            _id = value;
        }
    }
    private int _id;

    public DateTime DateAndTime
    {
        get => _dateAndTime;
        set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("DateAndTime cannot be in the future", nameof(DateAndTime));
            _dateAndTime = value;
        }
    }
    private DateTime _dateAndTime;
    
    private double totalAmount { get; }
    public bool IsDelivered { get; set; }

    public Order(int id, DateTime dateTime, bool isDelivered, 
        Customer? customer = null, 
        Payment.Payment? payment = null, 
        Review? review = null)
    {
        Id = id;
        DateAndTime = dateTime;
        IsDelivered = isDelivered;
        
        AddInstance(this);
        if (payment != null)
            AddPayment(payment);
        if (review != null)
            SetReview(review);
        if (customer != null)
            SetCustomer(customer);
    }
    
    public Order(){}
    
    public void AddDeliveryEmployee(DeliveryEmpl delivery)
    {
        ArgumentNullException.ThrowIfNull(delivery);

        if (DeliveryEmpl == delivery) return;

        if (DeliveryEmpl != null)
        {
            throw new InvalidOperationException("Order is already assigned to another Delivery Person.");
        }

        DeliveryEmpl = delivery;
        delivery.AddOrderInternal(this);
    }

    public void RemoveDeliveryEmployee()
    {
        if (DeliveryEmpl == null) return;

        var currentDelivery = DeliveryEmpl;
        DeliveryEmpl = null;
        currentDelivery.RemoveOrderInternal(this);
    }

    public void UpdateDeliveryEmployee(DeliveryEmpl newDelivery)
    {
        ArgumentNullException.ThrowIfNull(newDelivery);

        RemoveDeliveryEmployee();
        AddDeliveryEmployee(newDelivery);
    }
    
    public void AddPayment(Payment.Payment payment)
    {
        ArgumentNullException.ThrowIfNull(payment);

        if (payment.Order != null && !Equals(payment.Order, this))
        {
            throw new InvalidOperationException("Payment is already assigned to another Order");
        }

        payment.SetOrder(this);
    }

    internal void AddPaymentInternal(Payment.Payment payment)
    {
        Payment = payment;

        UpdateInstance(this);
    }

    public void AddMenuItem(AbstractMenuItem menuItem, int quantity)
    {
        ArgumentNullException.ThrowIfNull(menuItem);

        var association = new AbstractMenuItemOrderAssociation(menuItem, this, quantity);
        _menuItemAssociations.Add(association);
        menuItem.AddOrderAssociationInternal(association);
    }

    public void RemoveMenuItem(AbstractMenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);
        
        var association = _menuItemAssociations.FirstOrDefault(a => a.MenuItem == menuItem);
        if (association == null) return;

        _menuItemAssociations.Remove(association);
        menuItem.RemoveOrderAssociationInternal(association);
    }

    internal void AddMenuItemAssociationInternal(AbstractMenuItemOrderAssociation association)
    {
        if(!_menuItemAssociations.Contains(association))
            _menuItemAssociations.Add(association);
    }

    internal void RemoveMenuItemAssociationInternal(AbstractMenuItemOrderAssociation association)
    {
        _menuItemAssociations.Remove(association);
    }

    public void SetReview(Review review)
    {
        ArgumentNullException.ThrowIfNull(review);

        if (review == Review) return;
        
        if (review.Order != null && review.Order != this)
        {
            throw new InvalidOperationException("Review is already assigned to another Order");
        }

        review.SetOrder(this);
    }
    
    internal void SetReviewInternal(Review review)
    {
        Review = review;
    }

    public void SetCustomer(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        if (customer == Customer) return; // No need to reassign the same Order

        if (Customer != null)
            throw new InvalidOperationException("Order is already assigned to another Customer");

        if (customer.Orders.Contains(this)) return;

        Customer = customer;
        customer.AddOrderInternal(this);
    }
    
    protected internal override void RemoveInstance(Order instance)
    {
        Payment.RemoveInstance();

        base.RemoveInstance(instance);
    }
}