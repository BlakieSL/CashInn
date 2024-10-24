using CashInn.Enum;

namespace CashInn.Model;

public class Chef : AbstractEmployee, IAbstractKitchenEmployee
{
    public override string EmployeeType => "Chef";
    public string SpecialtyCuisine { get; set; }
    public int YearsOfExperience { get; set; }
    public int MichelinStars { get; set; }
    private double ExperienceBonus { get; }
    public Chef(int id, string name, double salary, DateTime hireDate, DateTime shiftStart, DateTime shiftEnd,
        StatusEmpl status, bool isBranchManager, string specialtyCuisine, int experienceLevel, int michelinStars,
        DateTime? layoffDate = null)
        : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, layoffDate)
    {
        SpecialtyCuisine = specialtyCuisine;
        YearsOfExperience = experienceLevel;
        MichelinStars = michelinStars;
        ExperienceBonus = YearsOfExperience * 0.04 * salary;
    }
    
    protected override object ToSerializableObject()
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