using CashInn.Enum;

namespace CashInn.Model;

public class Chef : Employee
{
    public string SpecialtyCuisine { get; set; }
    public int ExperienceLevel { get; set; }
    public List<Cook> ManagedCooks { get; set; }
    
    public Chef(string name, double salary, DateTime hireDate, DateTime shiftStart, DateTime shiftEnd,
        StatusEmpl status, string specialtyCuisine, int experienceLevel, DateTime? layoffDate = null)
        : base(name, "Chef", salary, hireDate, shiftStart, shiftEnd, status, layoffDate)
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