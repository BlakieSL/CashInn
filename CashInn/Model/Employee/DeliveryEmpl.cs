using CashInn.Enum;
using CashInn.Model.FlexibleEmplSetup;

namespace CashInn.Model.Employee;

public class DeliveryEmpl : AbstractEmployee, IDeliveryEmpl
{
    private readonly List<Order> _assignedOrders = [];
    public IEnumerable<Order> AssignedOrders => _assignedOrders.AsReadOnly();
    public override string EmployeeType => "DeliveryEmpl";
    private string _vehicle;
    public string Vehicle
    {
        get => _vehicle;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Vehicle cannot be null or empty", nameof(Vehicle));
            _vehicle = value;
        }
    }

    private string _deliveryArea;
    public string DeliveryArea
    {
        get => _deliveryArea;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Delivery area cannot be null or empty", nameof(DeliveryArea));
            _deliveryArea = value;
        }
    }

    public DeliveryEmpl(int id, string name, double salary, DateTime hireDate, 
        DateTime shiftStart, DateTime shiftEnd, StatusEmpl status, bool isBranchManager, string vehicle, 
        string deliveryArea, Branch employerBranch, Branch? managedBranch = null, DateTime? layoffDate = null)
        : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, employerBranch, managedBranch, layoffDate)
    {
        Vehicle = vehicle;
        DeliveryArea = deliveryArea;
        
        AddInstance();
    }
    
    public override object ToSerializableObject()
    {
        return new
        {
            Id,
            Name,
            Salary,
            HireDate,
            ShiftStart,
            ShiftEnd,
            Status,
            IsBranchManager,
            LayoffDate,
            Vehicle,
            DeliveryArea,
            EmployeeType,
            // EmployerBranch,
            // ManagedBranch
        };
    }
    
    public void AddOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (order.DeliveryEmpl != null && order.DeliveryEmpl != this)
        {
            throw new InvalidOperationException("Order is already assigned to another Delivery Person.");
        }

        order.AddDeliveryEmployee(this);
    }

    public void RemoveOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        if (!_assignedOrders.Contains(order)) return;

        order.RemoveDeliveryEmployee();
    }

    public void UpdateOrder(Order oldOrder, Order newOrder)
    {
        ArgumentNullException.ThrowIfNull(oldOrder);
        ArgumentNullException.ThrowIfNull(newOrder);

        if(!_assignedOrders.Contains(oldOrder))
            throw new InvalidOperationException("Delivery person is not assigned to oldOrder.");

        RemoveOrder(oldOrder);
        AddOrder(newOrder);
    }
    
    internal void AddOrderInternal(Order order)
    {
        if (!_assignedOrders.Contains(order))
        {
            _assignedOrders.Add(order);
        }
    }

    internal void RemoveOrderInternal(Order order)
    {
        _assignedOrders.Remove(order);
    }
}