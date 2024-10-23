using System.Text.Json.Serialization;
using CashInn.Enum;
using CashInn.Helper;

namespace CashInn.Model;

public class Employee
{
    private static readonly ICollection<Employee> Employees = new List<Employee>();
    
    public int Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public double Salary { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? LayoffDate { get; set; }
    public DateTime ShiftStart { get; set; }
    public DateTime ShiftEnd { get; set; }
    public StatusEmpl Status { get; set; }

    public Branch Branch { get; set; }

    public Employee()
    {
        
    }   
    
    protected Employee(int id, string name, string role, double salary, DateTime hireDate, DateTime shiftStart,
        DateTime shiftEnd, StatusEmpl status, DateTime? layoffDate = null)
    {
        Id = id;
        Name = name;
        Role = role;
        Salary = salary;
        HireDate = hireDate;
        ShiftStart = shiftStart;
        ShiftEnd = shiftEnd;
        Status = status;
        LayoffDate = layoffDate;
    }
    
    public static void SaveExtent(string filepath)
    {
        Saver.Serialize(Employees, filepath);
    }

    public static void LoadExtent(string filepath)
    {
        var deserializedEmployees = Saver.Deserialize<List<Employee>>(filepath);
        Employees.Clear();
        foreach (var employee in deserializedEmployees)
        {
            Employees.Add(employee);
        }
    }
    
    public static void SaveEmployee(Employee employee)
    {
        Employees.Add(employee);
    }
    
    public static ICollection<Employee> GetAllEmployees()
    {
        return Employees.ToList();
    }
}