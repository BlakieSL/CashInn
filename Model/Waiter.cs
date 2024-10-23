using CashInn.Enum;
using CashInn.Model.FlexibleEmplSetup;

namespace CashInn.Model;

public class Waiter : Employee, IWaiterEmpl
{ 
    public double TipsEarned { get; set; }
    
    public Waiter(int id, string name, double salary, DateTime hireDate, DateTime shiftStart, DateTime shiftEnd,
        StatusEmpl status, double tipsEarned, DateTime? layoffDate = null)
        : base(id, name, "Chef", salary, hireDate, shiftStart, shiftEnd, status, layoffDate)
    {
        TipsEarned = tipsEarned;
    }
}