using CashInn.Enum;
using CashInn.Model.FlexibleEmplSetup;

namespace CashInn.Model;

public class FlexibleEmpl : AbstractEmployee, IDeliveryEmpl, IWaiterEmpl
{
    public override string EmployeeType => "FlexibleEmpl";

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

    private double _tipsEarned;
    public double TipsEarned
    {
        get => _tipsEarned;
        set
        {
            if (value < 0)
                throw new ArgumentException("Tips earned cannot be negative", nameof(TipsEarned));
            _tipsEarned = value;
        }
    }
    public FlexibleEmpl(int id, string name, double salary, DateTime hireDate, DateTime shiftStart, DateTime shiftEnd,
        StatusEmpl status, bool isBranchManager, string vehicle, string deliveryArea, double tipsEarned,
        DateTime? layoffDate = null)
        : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, layoffDate)
    {
        Vehicle = vehicle;
        DeliveryArea = deliveryArea;
        TipsEarned = tipsEarned;
        ShiftStart = shiftStart;
        ShiftEnd = shiftEnd;
    }
    
    protected override object ToSerializableObject()
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
            TipsEarned,
            EmployeeType
        };
    }
}