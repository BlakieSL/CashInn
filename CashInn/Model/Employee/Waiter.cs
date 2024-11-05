using CashInn.Enum;
using CashInn.Model.FlexibleEmplSetup;

namespace CashInn.Model.Employee;

public class Waiter : AbstractEmployee, IWaiterEmpl
{ 
    public override string EmployeeType => "Waiter";

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
    public Waiter(int id, string name, double salary, DateTime hireDate, DateTime shiftStart, 
        DateTime shiftEnd, StatusEmpl status, bool isBranchManager,  double tipsEarned,
        DateTime? layoffDate = null)
        : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager,layoffDate)
    {
        TipsEarned = tipsEarned;
        
        SaveEmployee(this);
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
            TipsEarned,
            EmployeeType
        };
    }
}