using CashInn.Enum;

namespace CashInn.Model.Employee;

public class Chef : AbstractEmployee, IKitchenEmpl
{
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
            CalculateBonus();
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
    
    private double ExperienceBonus { get; set; }
    
    public Chef(int id, string name, double salary, DateTime hireDate, DateTime shiftStart, DateTime shiftEnd,
        StatusEmpl status, bool isBranchManager, string specialtyCuisine, int experienceLevel, int michelinStars,
        DateTime? layoffDate = null)
        : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, layoffDate)
    {
        SpecialtyCuisine = specialtyCuisine;
        YearsOfExperience = experienceLevel;
        MichelinStars = michelinStars;
        CalculateBonus();
        
        SaveEmployee(this);
    }

    private void CalculateBonus()
    {
        ExperienceBonus = YearsOfExperience * 0.04 * Salary;
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
}