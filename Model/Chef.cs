using CashInn.Enum;

namespace CashInn.Model;

public class Chef : Employee
{
    public string SpecialtyCuisine { get; set; }
    public int ExperienceLevel { get; set; }
    public List<Cook> ManagedCooks { get; set; }
    
    public Chef(int id, string name, double salary, DateTime hireDate, DateTime shiftStart, DateTime shiftEnd,
        StatusEmpl status, Branch branch,  string specialtyCuisine, int experienceLevel, DateTime? layoffDate = null)
        : base(id, name, "Chef", salary, hireDate, shiftStart, shiftEnd, status, branch, layoffDate)
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