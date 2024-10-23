using System.Text.Json.Serialization;
using CashInn.Enum;
using CashInn.Helper;

namespace CashInn.Model;

public class Employee
{
    private static readonly ICollection<Employee> Employees = new List<Employee>();
    
    public int Id { get; set; }
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be null or empty", nameof(Name));
            _name = value;
        }
    }
    private string _name;

    public string Role
    {
        get => _role;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Role cannot be null or empty", nameof(Role));
            _role = value;
        }
    }
    private string _role;

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
        DateTime shiftEnd, StatusEmpl status, Branch branch, DateTime? layoffDate = null)
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
        Branch = branch;
    }
    
    public static void SaveExtent(string filepath)
    {
        Saver.Serialize(Employees, filepath);
    }

    public static void LoadExtent(string filepath)
    {
        var deserializedEmployees = Saver.Deserialize<List<Employee>>(filepath);
        Employees.Clear();
        if (deserializedEmployees != null)
            foreach (var employee in deserializedEmployees)
            {
                Employees.Add(employee);
            }
    }
    
    public static void SaveEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);
        Employees.Add(employee);
    }
    
    public static ICollection<Employee> GetAllEmployees()
    {
        return Employees.ToList();
    }
}