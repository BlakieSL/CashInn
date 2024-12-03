﻿using CashInn.Enum;

namespace CashInn.Model.Employee;

public class Cook : AbstractEmployee, IKitchenEmpl
{
    public Chef? Manager { get; private set; }

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
        int yearsOfExperience, string station, DateTime? layoffDate = null, Chef? manager = null)
        : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, layoffDate)
    {
        SpecialtyCuisine = specialtyCuisine;
        YearsOfExperience = yearsOfExperience;
        Station = station;
        if (manager != null) AddManager(manager);

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
}