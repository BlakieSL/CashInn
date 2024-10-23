using CashInn.Enum;
using CashInn.Model.FlexibleEmplSetup;

namespace CashInn.Model;

public class DeliveryEmpl : Employee, IDeliveryEmpl
{
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

    public DeliveryEmpl(int id, string name, string role, double salary, DateTime hireDate, 
        DateTime shiftStart, DateTime shiftEnd, StatusEmpl status, bool isBranchManager, string vehicle, 
        string deliveryArea, DateTime? layoffDate = null)
        : base(id, name, "DeliveryEmpl", salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, layoffDate)
    {
        Vehicle = vehicle;
        DeliveryArea = deliveryArea;
    }
}