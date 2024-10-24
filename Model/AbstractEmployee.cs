using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using CashInn.Enum;
using CashInn.Helper;

namespace CashInn.Model;

public abstract class AbstractEmployee
{
    [JsonIgnore]
    public abstract string EmployeeType { get; }
    
    private static readonly ICollection<AbstractEmployee> Employees = new List<AbstractEmployee>();
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
    private double _salary { get; set; }
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
    private DateTime _hireDate { get; set;}
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
    
    protected AbstractEmployee(int id, string name, double salary, DateTime hireDate, DateTime shiftStart,
        DateTime shiftEnd, StatusEmpl status, bool isBranchManager, DateTime? layoffDate = null)
    {
        Id = id;
        Name = name;
        Salary = salary;
        HireDate = hireDate;
        ShiftEnd = shiftEnd;
        ShiftStart = shiftStart;
        Status = status;
        LayoffDate = layoffDate;
        IsBranchManager = isBranchManager;
    }

    public static void SaveExtent(string filepath)
    {
        Saver.Serialize(Employees.Select(e => e.ToSerializableObject()).ToList(), filepath);
    }
    
    protected abstract object ToSerializableObject();

    public static void LoadExtent(string filepath)
    {
        var deserializedEmployees = Saver.Deserialize<List<dynamic>>(filepath);
        Employees.Clear();

        if (deserializedEmployees == null) return;

        foreach (var employeeData in deserializedEmployees)
        {
            int id = employeeData.GetProperty("Id").GetInt32();
            string name = employeeData.GetProperty("Name").GetString();
            double salary = employeeData.GetProperty("Salary").GetDouble();
            DateTime hireDate = employeeData.GetProperty("HireDate").GetDateTime();
            DateTime shiftStart = employeeData.GetProperty("ShiftStart").GetDateTime();
            DateTime shiftEnd = employeeData.GetProperty("ShiftEnd").GetDateTime();
            
            var statusString = employeeData.GetProperty("Status").GetString();
            StatusEmpl emplStatus;
            if (System.Enum.TryParse<StatusEmpl>(statusString, out StatusEmpl status))
            {
                emplStatus = status;
            } else
            {
                throw new ArgumentException($"Invalid status value: {statusString}");
            }
            
            bool isBranchManager = employeeData.GetProperty("IsBranchManager").GetBoolean();
            
            DateTime? layoffDate = null;
            if (employeeData.TryGetProperty("LayoffDate", out JsonElement layoffDateProperty) && layoffDateProperty.ValueKind != JsonValueKind.Null)
            {
                layoffDate = layoffDateProperty.GetDateTime();
            }
            
            string employeeType = employeeData.GetProperty("EmployeeType").GetString();
            AbstractEmployee abstractEmployee;
            if (employeeType == "Cook")
            {
                abstractEmployee = new Cook(
                    id,
                    name,
                    salary,
                    hireDate,
                    shiftStart,
                    shiftEnd,
                    emplStatus,
                    isBranchManager,
                    employeeData.GetProperty("SpecialtyCuisine").ToString(),
                    employeeData.GetProperty("YearsOfExperience").GetInt32(),
                    employeeData.GetProperty("Station").ToString(),
                    layoffDate
                );
            }
            else if (employeeType == "Chef")
            {
                abstractEmployee = new Chef(
                    id,
                    name,
                    salary,
                    hireDate,
                    shiftStart,
                    shiftEnd,
                    emplStatus,
                    isBranchManager,
                    employeeData.GetProperty("SpecialtyCuisine").ToString(),
                    employeeData.GetProperty("YearsOfExperience").GetInt32(),
                    employeeData.GetProperty("MichelinStars").GetInt32(),
                    layoffDate
                );
            } 
            else if (employeeType == "DeliveryEmpl")
            {
                abstractEmployee = new DeliveryEmpl(
                    id,
                    name,
                    salary,
                    hireDate,
                    shiftStart,
                    shiftEnd,
                    emplStatus,
                    isBranchManager,
                    employeeData.GetProperty("Vehicle").ToString(),
                    employeeData.GetProperty("DeliveryArea").ToString(),
                    layoffDate
                );
            }
            else if (employeeType == "FlexibleEmpl")
            {
                abstractEmployee = new FlexibleEmpl(
                    id,
                    name,
                    salary,
                    hireDate,
                    shiftStart,
                    shiftEnd,
                    emplStatus,
                    isBranchManager,
                    employeeData.GetProperty("Vehicle").ToString(),
                    employeeData.GetProperty("DeliveryArea").ToString(),
                    employeeData.GetProperty("TipsEarned").GetDouble(),
                    layoffDate
                );
            }
            else if (employeeType == "Waiter")
            {
                abstractEmployee = new Waiter(
                    id,
                    name,
                    salary,
                    hireDate,
                    shiftStart,
                    shiftEnd,
                    emplStatus,
                    isBranchManager,
                    employeeData.GetProperty("TipsEarned").GetDouble(),
                    layoffDate
                );
            }
            else
            {
                throw new InvalidOperationException("Unknown employee type.");
            }

            Employees.Add(abstractEmployee);
        }
    }
    
    public static void SaveEmployee(AbstractEmployee abstractEmployee)
    {
        ArgumentNullException.ThrowIfNull(abstractEmployee);
        Employees.Add(abstractEmployee);
    }
    
    public static ICollection<AbstractEmployee> GetAllEmployees()
    {
        return Employees.ToImmutableList();
    }
}