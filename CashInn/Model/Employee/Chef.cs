using CashInn.Enum;

namespace CashInn.Model.Employee;

public class Chef : AbstractEmployee, IKitchenEmpl
{
    private readonly List<Cook> _managedCooks = [];
    public IEnumerable<Cook> ManagedCooks => _managedCooks.AsReadOnly();
    public Kitchen? ManagedKitchen { get; private set; }
    public override string EmployeeType => "Chef";
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

    private int _michelinStars;
    public int MichelinStars
    {
        get => _michelinStars;
        set
        {
            if (value < 0 || value > 3)
                throw new ArgumentException("Michelin stars must be between 0 and 3", nameof(MichelinStars));
            _michelinStars = value;
        }
    }

    public double ExperienceBonus
    {
        get =>  YearsOfExperience * 0.04 * Salary;
    }

    public Chef(int id, string name, double salary, DateTime hireDate, DateTime shiftStart, DateTime shiftEnd,
        StatusEmpl status, bool isBranchManager, string specialtyCuisine, int experienceLevel, int michelinStars,
        Branch employerBranch, Branch? managedBranch = null, DateTime? layoffDate = null, Kitchen? kitchen = null)
        : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, employerBranch, managedBranch, layoffDate)
    {
        SpecialtyCuisine = specialtyCuisine;
        YearsOfExperience = experienceLevel;
        MichelinStars = michelinStars;

        if (kitchen != null)
        {
            ManagedKitchen = kitchen;
        }
        
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
            MichelinStars,
            EmployeeType,
            // EmployerBranch,
            // ManagedBranch
        };
    }

    public void AddCook(Cook cook)
    {
        ArgumentNullException.ThrowIfNull(cook);

        if (cook.Manager != null && cook.Manager != this)
        {
            throw new InvalidOperationException("Cook is already managed by another chef");
        }

        cook.AddManager(this);
    }

    public void RemoveCook(Cook cook)
    {
        ArgumentNullException.ThrowIfNull(cook);
        if (!_managedCooks.Contains(cook)) return;

        cook.RemoveManager();
    }

    public void UpdateCook(Cook oldCook, Cook newCook)
    {
        ArgumentNullException.ThrowIfNull(oldCook);
        ArgumentNullException.ThrowIfNull(newCook);

        if(!_managedCooks.Contains(oldCook))
            throw new InvalidOperationException("Cook is not managed by this chef");

        RemoveCook(oldCook);
        AddCook(newCook);
    }

    internal void AddCookInternal(Cook cook)
    {
        if (!_managedCooks.Contains(cook))
        {
            _managedCooks.Add(cook);
        }
    }

    internal void RemoveCookInternal(Cook cook)
    {
        _managedCooks.Remove(cook);
    }
    
    //--------------------------------------------
    public void AddKitchen(Kitchen? kitchen)
    {
        ArgumentNullException.ThrowIfNull(kitchen);

        if (ManagedKitchen == kitchen) return;

        kitchen.AddManager(this);
    }

    public void RemoveKitchen()
    {
        ArgumentNullException.ThrowIfNull(ManagedKitchen);
        
        ManagedKitchen.RemoveManager();
    }

    internal void AddKitchenInternal(Kitchen? kitchen)
    {
        ManagedKitchen ??= kitchen;
    }
    
    internal void RemoveKitchenInternal()
    {
        ManagedKitchen = null;
    }
}