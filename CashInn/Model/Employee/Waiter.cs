using CashInn.Enum;
using CashInn.Model.FlexibleEmplSetup;

namespace CashInn.Model.Employee;

public class Waiter : AbstractEmployee, IWaiterEmpl
{ 
    private readonly List<Table> _assignedTables = [];
    public IEnumerable<Table> AssignedTables => _assignedTables.AsReadOnly();
    public override string EmployeeType => "Waiter";

    private double _tipsEarned;
    public double TipsEarned
    {
        get => _tipsEarned;
        set
        {
            if (value < 0)
                throw new ArgumentException("Tips earned cannot be negative", nameof(TipsEarned));
            _tipsEarned = value;
        }
    }
    public Waiter(int id, string name, double salary, DateTime hireDate, DateTime shiftStart, 
        DateTime shiftEnd, StatusEmpl status, bool isBranchManager,  double tipsEarned,
        Branch employerBranch, Branch? managedBranch = null, DateTime? layoffDate = null)
        : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, employerBranch, managedBranch, layoffDate)
    {
        TipsEarned = tipsEarned;
        
        AddInstance();
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
            TipsEarned,
            EmployeeType,
            // EmployerBranch,
            // ManagedBranch
        };
    }
    
    public void AddTable(Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        if (table.Waiter != null && table.Waiter != this)
        {
            throw new InvalidOperationException("Table is already assigned to another Waiter.");
        }

        table.AddWaiter(this);
    }

    public void RemoveTable(Table table)
    {
        ArgumentNullException.ThrowIfNull(table);
        if (!_assignedTables.Contains(table)) return;

        table.RemoveWaiter();
    }

    public void UpdateTable(Table oldTable, Table newTable)
    {
        ArgumentNullException.ThrowIfNull(oldTable);
        ArgumentNullException.ThrowIfNull(newTable);

        if(!_assignedTables.Contains(oldTable))
            throw new InvalidOperationException("Waiter does not manage oldTable.");

        RemoveTable(oldTable);
        AddTable(newTable);
    }
    
    internal void AddTableInternal(Table table)
    {
        if (!_assignedTables.Contains(table))
        {
            _assignedTables.Add(table);
        }
    }

    internal void RemoveTableInternal(Table table)
    {
        _assignedTables.Remove(table);
    }
}