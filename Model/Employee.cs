using CashInn.Enum;

namespace CashInn.Model;

public class Employee
{
    private static readonly List<Employee> Employees = new List<Employee>();
    private static int _idCounter = 1;
    
    public int Id { get; private set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public double Salary { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? LayoffDate { get; set; }
    public DateTime ShiftStart { get; set; }
    public DateTime ShiftEnd { get; set; }
    public StatusEmpl Status { get; set; }

    protected Employee(string name, string role, double salary, DateTime hireDate, DateTime shiftStart,
        DateTime shiftEnd, StatusEmpl status, DateTime? layoffDate = null)
    {
        Id = _idCounter++;
        Name = name;
        Role = role;
        Salary = salary;
        HireDate = hireDate;
        ShiftStart = shiftStart;
        ShiftEnd = shiftEnd;
        Status = status;
        LayoffDate = layoffDate;
        
        Employees.Add(this);
    }

    public static List<Employee> GetAllEmployees()
    {
        return [..Employees];
    }
    
}