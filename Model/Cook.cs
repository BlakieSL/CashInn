using CashInn.Enum;

namespace CashInn.Model;

public class Cook : Employee
{
    public Cook(int id, string name, string role, double salary, DateTime hireDate, DateTime shiftStart,
        DateTime shiftEnd, StatusEmpl status, Branch branch,  DateTime? layoffDate = null) 
        : base(id, name, "Cook", salary, hireDate, shiftStart, shiftEnd, status, branch, layoffDate)
    {
    }
}