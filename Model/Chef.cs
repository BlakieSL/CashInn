using CashInn.Enum;

namespace CashInn.Model;

public class Chef : Employee
{
    public string SpecialtyCuisine { get; set; }

    private int _experienceLevel;
    public int ExperienceLevel
    {
        get => _experienceLevel;
        set
        {
            if (value < 0)
                throw new ArgumentException("Experience level cannot be negative", nameof(ExperienceLevel));
            _experienceLevel = value;
        }
    }
    public List<Cook> ManagedCooks { get; set; }
    
    public Chef(int id, string name, double salary, DateTime hireDate, DateTime shiftStart, DateTime shiftEnd,
        StatusEmpl status, bool isBranchManager, string specialtyCuisine, int experienceLevel,
        DateTime? layoffDate = null)
        : base(id, name, "Chef", salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, layoffDate)
    {
        SpecialtyCuisine = specialtyCuisine;
        ExperienceLevel = experienceLevel;
        ManagedCooks = new List<Cook>();
    }

    public void AddCook(Cook cook)
    {
        ManagedCooks.Add(cook);
    }

    public bool RemoveCook(Cook cook)
    {
        return ManagedCooks.Remove(cook);
    }
}