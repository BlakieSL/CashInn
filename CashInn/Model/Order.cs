using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Order : ClassExtent<Order>
{
    public DeliveryEmpl? DeliveryEmpl { get; private set; }
    protected override string FilePath => ClassExtentFiles.OrdersFile;
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

    public Order(int id, DateTime dateTime, bool isDelivered)
    {
        Id = id;
        DateAndTime = dateTime;
        IsDelivered = isDelivered;
        
        AddInstance(this);
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
}