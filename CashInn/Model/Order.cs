using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Order : ClassExtent<Order>
{
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
    }
    
    public Order(){}
}