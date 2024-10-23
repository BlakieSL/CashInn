using CashInn.Enum;
using CashInn.Model.FlexibleEmplSetup;

namespace CashInn.Model;

public class FlexibleEmpl : Employee, IDeliveryEmpl, IWaiterEmpl 
{
    public string Vehicle { get; set; }
    public string DeliveryArea { get; set; }
    
    public double TipsEarned { get; set; }

    public FlexibleEmpl(int id, string name, double salary, DateTime hireDate, DateTime shiftStart, DateTime shiftEnd,
        StatusEmpl status, string vehicle, string deliveryArea, DateTime? layoffDate = null)
        : base(id, name, "Flexible Employee", salary, hireDate, shiftStart, shiftEnd, status, layoffDate)
    {
        Vehicle = vehicle;
        DeliveryArea = deliveryArea;
        ShiftStart = shiftStart;
        ShiftEnd = shiftEnd;
    }
}