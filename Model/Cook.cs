using CashInn.Enum;

namespace CashInn.Model;

public class Cook : Employee
{
    public Cook(string name, string role, double salary, DateTime hireDate, DateTime shiftStart,
        DateTime shiftEnd, StatusEmpl status, DateTime? layoffDate = null) 
        : base(name, "Cook", salary, hireDate, shiftStart, shiftEnd, status, layoffDate)
    {
    }
}