using CashInn.Enum;
using CashInn.Model.FlexibleEmplSetup;

namespace CashInn.Model;

public class DeliveryEmpl : Employee, IDeliveryEmpl
{
    public string Vehicle { get; set; }
    public string DeliveryArea { get; set; }


    public DeliveryEmpl(string name, string role, double salary, DateTime hireDate, 
        DateTime shiftStart, DateTime shiftEnd, StatusEmpl status, string vehicle, 
        string deliveryArea, DateTime? layoffDate = null)
        : base(name, "DeliveryEmpl", salary, hireDate, shiftStart, shiftEnd, status, layoffDate)
    {
        Vehicle = vehicle;
        DeliveryArea = deliveryArea;
        
    }
}