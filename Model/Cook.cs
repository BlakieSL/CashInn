using CashInn.Enum;
using CashInn.Helper;

namespace CashInn.Model;

public class Cook : AbstractEmployee, IAbstractKitchenEmployee
{
    public string SpecialtyCuisine { get; set; }
    public int YearsOfExperience { get; set; }
    public string Station { get; set; }
    public override string EmployeeType => "Cook";
    public Cook(int id, string name, double salary, DateTime hireDate, DateTime shiftStart,
        DateTime shiftEnd, StatusEmpl status, bool isBranchManager, string specialtyCuisine, int yearsOfExperience, string station, DateTime? layoffDate = null) 
        : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, layoffDate)
    {
        SpecialtyCuisine = specialtyCuisine;
        YearsOfExperience = yearsOfExperience;
        Station = station;
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
            Station,
            EmployeeType
        };
    }
}