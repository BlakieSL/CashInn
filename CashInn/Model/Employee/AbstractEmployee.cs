using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using CashInn.Enum;
using CashInn.Helper;

namespace CashInn.Model.Employee;

public abstract class AbstractEmployee
{
    private static readonly string _filepath = ClassExtentFiles.EmployeesFile;
    [JsonIgnore]
    public abstract string EmployeeType { get; }
    private static readonly ICollection<AbstractEmployee> Instances = new List<AbstractEmployee>();
    public Branch EmployerBranch { get; private set; }
    public Branch? ManagedBranch { get; private set; }
    public int Id 
    {
        get => _id;
        set
        {
            if (value < 0)
                throw new ArgumentException("Id cannot be less than 0", nameof(Id));
            _id = value;
        }
    }
    private int _id;
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
    private double _salary;

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
    private DateTime _hireDate;

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
    private DateTime _shiftStart;

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
    private DateTime _shiftEnd;
    
    public StatusEmpl Status { get; set; }
    public bool IsBranchManager { get; set; }
    
    protected AbstractEmployee(int id, string name, double salary, DateTime hireDate, DateTime shiftStart,
        DateTime shiftEnd, StatusEmpl status, bool isBranchManager, Branch employerBranch, 
        Branch? managedBranch = null, DateTime? layoffDate = null)
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
        
        if (employerBranch == null)
            throw new InvalidOperationException("Branch cannot be null");
        if (managedBranch != null)
            ManagedBranch = managedBranch;
        
        EmployerBranch = employerBranch;
        employerBranch.AddEmployeeInternal(this);
    }

    public static void SaveExtent()
    {
        Saver.Serialize(Instances.Select(e => e.ToSerializableObject()).ToList(), _filepath);
    }
    
    public abstract object ToSerializableObject();

    public static void LoadExtent()
    {
        ClearExtent();
        var deserializedEmployees = Saver.Deserialize<List<dynamic>>(_filepath);

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
            if (employeeData.TryGetProperty("LayoffDate", out JsonElement layoffDateProperty) 
                && layoffDateProperty.ValueKind != JsonValueKind.Null)
            {
                layoffDate = layoffDateProperty.GetDateTime();
            }
            
            string employeeType = employeeData.GetProperty("EmployeeType").GetString();
            Branch employerBranch = JsonSerializer.Deserialize<Branch>(employeeData.GetProperty("Branch").GetRawText());
            if (employerBranch == null)
            {
                throw new InvalidOperationException("EmployerBranch cannot be null.");
            }
            Branch managedBranch = JsonSerializer.Deserialize<Branch>(employeeData.GetProperty("Branch").GetRawText());
            AbstractEmployee abstractEmployee = employeeType switch
            {
                "Cook" => new Cook(id, name, salary, hireDate, shiftStart, shiftEnd, emplStatus, isBranchManager,
                    employeeData.GetProperty("SpecialtyCuisine").ToString(),
                    employeeData.GetProperty("YearsOfExperience").GetInt32(),
                    employeeData.GetProperty("Station").ToString(), employerBranch, managedBranch, layoffDate),
                
                "Chef" => new Chef(id, name, salary, hireDate, shiftStart, shiftEnd, emplStatus, isBranchManager,
                    employeeData.GetProperty("SpecialtyCuisine").ToString(),
                    employeeData.GetProperty("YearsOfExperience").GetInt32(),
                    employeeData.GetProperty("MichelinStars").GetInt32(), employerBranch, managedBranch, layoffDate),
                
                "DeliveryEmpl" => new DeliveryEmpl(id, name, salary, hireDate, shiftStart, shiftEnd, emplStatus,
                    isBranchManager, employeeData.GetProperty("Vehicle").ToString(),
                    employeeData.GetProperty("DeliveryArea").ToString(), employerBranch, managedBranch, layoffDate),
                
                "FlexibleEmpl" => new FlexibleEmpl(id, name, salary, hireDate, shiftStart, shiftEnd, emplStatus,
                    isBranchManager, employeeData.GetProperty("Vehicle").ToString(),
                    employeeData.GetProperty("DeliveryArea").ToString(),
                    employeeData.GetProperty("TipsEarned").GetDouble(), employerBranch, managedBranch, layoffDate),
                
                "Waiter" => new Waiter(id, name, salary, hireDate, shiftStart, shiftEnd, emplStatus, isBranchManager,
                    employeeData.GetProperty("TipsEarned").GetDouble(), employerBranch, managedBranch, layoffDate),
                
                _ => throw new InvalidOperationException("Unknown employee type.")
            };
        }
    }

    public void AddInstance()
    {
        ArgumentNullException.ThrowIfNull(this);
        Instances.Add(this);
    }
    
    public void RemoveInstance()
    {
        ArgumentNullException.ThrowIfNull(this);
        Instances.Remove(this);
    }
    
    public static ICollection<AbstractEmployee> GetAll()
    {
        return Instances.ToImmutableList();
    }

    public static void ClearExtent()
    {
        Instances.Clear();
    }

    public void AddEmployerBranch(Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);
        if (EmployerBranch == branch) return;

        if (EmployerBranch != null)
        {
            throw new InvalidOperationException("Employee is already employed by another Branch");
        }

        EmployerBranch = branch;
        branch.AddEmployeeInternal(this);
    }
    
    public void RemoveEmployerBranch()
    {
        EmployerBranch.RemoveEmployeeInternal(this);
        RemoveInstance();
    }
    
    public void AddManagedBranch(Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);

        if (ManagedBranch == branch) return;

        if (ManagedBranch != null)
        {
            throw new InvalidOperationException("Branch is already managed by another Employee");
        }

        ManagedBranch = branch;
        branch.AddManagerInternal(this);
    }
    
    public void RemoveManagedBranch()
    {
        if (ManagedBranch == null) return;

        var currentBranch = ManagedBranch;
        ManagedBranch = null;
        currentBranch.RemoveManagerInternal();
    }
}