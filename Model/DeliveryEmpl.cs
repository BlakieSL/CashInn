using CashInn.Enum;
using CashInn.Model.FlexibleEmplSetup;

namespace CashInn.Model;

public class DeliveryEmpl : Employee, IDeliveryEmpl
{
    public string Vehicle { get; set; }
    public string DeliveryArea { get; set; }


    public DeliveryEmpl(int id, string name, string role, double salary, DateTime hireDate, 
        DateTime shiftStart, DateTime shiftEnd, StatusEmpl status, bool isBranchManager, string vehicle, 
        string deliveryArea, DateTime? layoffDate = null)
        : base(id, name, "DeliveryEmpl", salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, layoffDate)
    {
        Vehicle = vehicle;
        DeliveryArea = deliveryArea;
        
    }
}