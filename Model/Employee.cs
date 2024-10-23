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
    public double _salary { get; set; }
    public double Salary
    {
        get => _salary;
        set
        {
            if (value < 0)
                throw new ArgumentException("Salary cannot be negative", nameof(Salary));
            _salary = value;
        }
    }
    public DateTime _hireDate { get; set; }
    public DateTime HireDate
    {
        get => _hireDate;
        set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("Hire date cannot be in the future", nameof(HireDate));
            _hireDate = value;
        }
    }
    public DateTime? LayoffDate
    {
        get => _layoffDate;
        set
        {
            if (value.HasValue && value.Value < HireDate)
                throw new ArgumentException("Layoff date cannot be before hire date", nameof(LayoffDate));
            _layoffDate = value;
        }
    }
    private DateTime? _layoffDate;

    private DateTime _shiftStart;
    public DateTime ShiftStart
    {
        get => _shiftStart;
        set
        {
            if (value > ShiftEnd)
                throw new ArgumentException("Shift start time cannot be after shift end time", nameof(ShiftStart));
            _shiftStart = value;
        }
    }

    private DateTime _shiftEnd;
    public DateTime ShiftEnd
    {
        get => _shiftEnd;
        set
        {
            if (value < ShiftStart)
                throw new ArgumentException("Shift end time cannot be before shift start time", nameof(ShiftEnd));
            _shiftEnd = value;
        }
    }

    public StatusEmpl Status { get; set; }

    public bool IsBranchManager { get; set; }

    [JsonIgnore]
    public Branch Branch { get; set; }

    public Employee() {}   
    
    protected Employee(int id, string name, string role, double salary, DateTime hireDate, DateTime shiftStart,
        DateTime shiftEnd, StatusEmpl status, bool isBranchManager, DateTime? layoffDate = null)
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
        IsBranchManager = isBranchManager;
    }
    
    public static void SaveExtent(string filepath)
    {
        Saver.Serialize(Employees, filepath);
    }

    public static void LoadExtent(string filepath)
    {
        var deserializedEmployees = Saver.Deserialize<List<Employee>>(filepath);
        Employees.Clear();
        
        if (deserializedEmployees == null) return;
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