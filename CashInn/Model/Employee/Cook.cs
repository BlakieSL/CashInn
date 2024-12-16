using CashInn.Enum;

namespace CashInn.Model.Employee;

public class Cook : AbstractEmployee, IKitchenEmpl
{
    public Chef? Manager { get; private set; }
    public Kitchen? Kitchen { get; private set; }
    private string _specialtyCuisine;
    public string SpecialtyCuisine
    {
        get => _specialtyCuisine;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Specialty cuisine cannot be null or empty", nameof(SpecialtyCuisine));
            _specialtyCuisine = value;
        }
    }

    private int _yearsOfExperience;
    public int YearsOfExperience
    {
        get => _yearsOfExperience;
        set
        {
            if (value < 0)
                throw new ArgumentException("Years of experience cannot be negative", nameof(YearsOfExperience));
            _yearsOfExperience = value;
        }
    }
    public string Station
    {
        get => _station;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Specialty cuisine cannot be null or empty", nameof(SpecialtyCuisine));
            _station = value;
        }
    }
    private string _station;

    public override string EmployeeType => "Cook";
    public Cook(
        int id, string name, double salary, DateTime hireDate, DateTime shiftStart,
        DateTime shiftEnd, StatusEmpl status, bool isBranchManager, string specialtyCuisine, 
        int yearsOfExperience, string station, Branch employerBranch, Branch? managedBranch = null, 
        DateTime? layoffDate = null, Chef? manager = null, Kitchen? kitchen = null)
        : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, employerBranch, managedBranch, layoffDate)
    {
        SpecialtyCuisine = specialtyCuisine;
        YearsOfExperience = yearsOfExperience;
        Station = station;
        if (manager != null) AddManager(manager);
        if (kitchen != null) AddKitchen(kitchen);

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
            SpecialtyCuisine,
            YearsOfExperience,
            Station,
            EmployeeType,
            // EmployerBranch,
            // ManagedBranch
        };
    }
    //--------------------------------------------
    public void AddManager(Chef manager)
    {
        ArgumentNullException.ThrowIfNull(manager);

        if (Manager == manager) return;

        if (Manager != null)
        {
            throw new InvalidOperationException("Cook is already managed by another Chef.");
        }

        Manager = manager;
        manager.AddCookInternal(this);
    }

    public void RemoveManager()
    {
        if (Manager == null) return;

        var currentManager = Manager;
        Manager = null;
        currentManager.RemoveCookInternal(this);
    }

    public void UpdateManager(Chef newManager)
    {
        ArgumentNullException.ThrowIfNull(newManager);

        RemoveManager();
        AddManager(newManager);
    }
    //--------------------------------------------
    public void AddKitchen(Kitchen kitchen)
    {
        ArgumentNullException.ThrowIfNull(kitchen);

        if (Kitchen == kitchen) return;

        if (Kitchen != null)
        {
            throw new InvalidOperationException("Cook is already assigned to another Kitchen.");
        }

        Kitchen = kitchen;
        kitchen.AddCookInternal(this);
    }
    
    public void RemoveKitchen()
    {
        if (Kitchen == null) return;

        var currentKitchen = Kitchen;
        Kitchen = null;
        currentKitchen.RemoveCookInternal(this);
    }
    public void UpdateKitchen(Kitchen newKitchen)
    {
        ArgumentNullException.ThrowIfNull(newKitchen);

        RemoveKitchen();
        AddKitchen(newKitchen);
    }
}