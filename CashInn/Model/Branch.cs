using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Branch : ClassExtent<Branch>
{
    private readonly List<AbstractEmployee> _employees = [];
    private readonly Dictionary<int, Table> _tables = [];
    [JsonIgnore] public IEnumerable<AbstractEmployee> Employees => _employees.AsReadOnly();
    [JsonIgnore] public ReadOnlyDictionary<int, Table> Tables => _tables.AsReadOnly();
    [JsonIgnore] public AbstractEmployee? Manager { get; private set; }
    [JsonIgnore] public Menu Menu { get; private set; }

    protected override string FilePath => ClassExtentFiles.BranchesFile;

    //need to get rid of ids
    public int Id
    {
        get => _id;
        set
        {
            if (value < 0)
                throw new ArgumentException("Id cannot be less than 0", nameof(Id));
            _id = value;

            UpdateInstance(this);
        }
    }

    private int _id;

    private string _location;

    public string Location
    {
        get => _location;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Location cannot be null or empty", nameof(Location));
            _location = value;

            UpdateInstance(this);
        }
    }

    private string _contactInfo;

    public string ContactInfo
    {
        get => _contactInfo;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Contact info cannot be null or empty", nameof(ContactInfo));
            _contactInfo = value;

            UpdateInstance(this);
        }
    }

    public Branch(int id, string location, string contactInfo)
    {
        Id = id;
        Location = location;
        ContactInfo = contactInfo;

        AddInstance(this);
    }

    public Branch()
    {
    }

    //--------------------------------------------
    public void AddEmployee(AbstractEmployee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (employee.EmployerBranch != null && !Equals(employee.EmployerBranch, this))
        {
            throw new InvalidOperationException("Employee is already employed by another Branch");
        }

        employee.AddEmployerBranch(this);
    }

    public void RemoveEmployee(AbstractEmployee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);
        if (!_employees.Contains(employee)) return;

        employee.RemoveEmployerBranch();
    }

    internal void AddEmployeeInternal(AbstractEmployee employee)
    {
        if (!_employees.Contains(employee))
        {
            _employees.Add(employee);
        }

        UpdateInstance(this);
    }

    internal void RemoveEmployeeInternal(AbstractEmployee employee)
    {
        _employees.Remove(employee);

        UpdateInstance(this);
    }

    public void AddManager(AbstractEmployee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (employee.ManagedBranch != null && !Equals(employee.ManagedBranch, this))
        {
            throw new InvalidOperationException("Employee is already employed by another Branch");
        }

        employee.AddManagedBranch(this);
    }

    public void RemoveManager()
    {
        Manager?.RemoveManagedBranch();
    }

    internal void AddManagerInternal(AbstractEmployee employee)
    {
        Manager = employee;

        UpdateInstance(this);
    }

    internal void RemoveManagerInternal()
    {
        Manager = null;

        UpdateInstance(this);
    }
    
    public void AddMenu(Menu menu)
    {
        ArgumentNullException.ThrowIfNull(menu);

        if (menu.Branch != null && !Equals(menu.Branch, this))
        {
            throw new InvalidOperationException("Menu is already assigned to another Branch");
        }

        menu.SetBranch(this);
    }

    internal void AddMenuInternal(Menu menu)
    {
        Menu = menu;

        UpdateInstance(this);
    }
    
    public void AddTable(Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        if (table.Branch != null && !Equals(table.Branch, this))
        {
            throw new InvalidOperationException("Table is already assigned to another Branch");
        }
        
        table.SetBranch(this);
    }
    
    internal void AddTableInternal(Table table)
    {
        if (_tables.ContainsKey(table.TableNumber))
            throw new InvalidOperationException("Table with such tableNumber is already stored in the Branch");
        
        _tables.Add(table.TableNumber, table);
        UpdateInstance(this);
    }
    
    public void RemoveTable(Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        if (table.Branch != null && !Equals(table.Branch, this))
        {
            throw new InvalidOperationException("Table is already assigned to another Branch");
        }
        
        table.RemoveBranch();
    }
    
    internal void RemoveTableInternal(Table table)
    {
        if (!_tables.ContainsKey(table.TableNumber))
            throw new InvalidOperationException("Table with such tableNumber is not stored in the Branch");
        
        _tables.Remove(table.TableNumber);
        UpdateInstance(this);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Branch other) return false;
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    protected internal override void RemoveInstance(Branch instance)
    {
        foreach (var table in Tables.Values)
        {
            table.RemoveInstance(table);
        }
        
        Menu.RemoveInstance(Menu);
        
        foreach (var employee in Employees)
        {
            employee.RemoveInstance();
        }

        base.RemoveInstance(instance);
    }
}