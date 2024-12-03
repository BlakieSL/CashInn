using CashInn.Enum;

namespace CashInn.Model.Employee;

public class Chef : AbstractEmployee, IKitchenEmpl
{
    private readonly List<Cook> _managedCooks = [];
    public IEnumerable<Cook> ManagedCooks => _managedCooks.AsReadOnly();
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
        DateTime? layoffDate = null)
        : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, layoffDate)
    {
        SpecialtyCuisine = specialtyCuisine;
        YearsOfExperience = experienceLevel;
        MichelinStars = michelinStars;

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
            EmployeeType
        };
    }

    public void AddCook(Cook cook)
    {
        ArgumentNullException.ThrowIfNull(cook);

        if (_managedCooks.Contains(cook)) return;

        if (cook.Manager != null && cook.Manager != this)
        {
            throw new InvalidOperationException("Cook is already managed by another chef");
        }

        _managedCooks.Add(cook);
        cook.AddManager(this);
    }

    public void RemoveCook(Cook cook)
    {
        ArgumentNullException.ThrowIfNull(cook);
        if (!_managedCooks.Contains(cook)) return;

        _managedCooks.Remove(cook);
        cook.RemoveManager();
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
}