﻿using CashInn.Enum;

namespace CashInn.Model.Employee;

public class Cook : AbstractEmployee, IKitchenEmpl
{
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
    public Cook(int id, string name, double salary, DateTime hireDate, DateTime shiftStart,
        DateTime shiftEnd, StatusEmpl status, bool isBranchManager, string specialtyCuisine, int yearsOfExperience, string station, DateTime? layoffDate = null) 
        : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, layoffDate)
    {
        SpecialtyCuisine = specialtyCuisine;
        YearsOfExperience = yearsOfExperience;
        Station = station;
        
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
            EmployeeType
        };
    }
}