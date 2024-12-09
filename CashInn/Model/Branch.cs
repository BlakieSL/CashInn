using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Branch : ClassExtent<Branch>
{
    private readonly List<AbstractEmployee> _employees = [];
    public IEnumerable<AbstractEmployee> Employees => _employees.AsReadOnly();
    public AbstractEmployee? Manager { get; private set; }
    protected override string FilePath => ClassExtentFiles.BranchesFile;
    public int Id 
    {
        get => _id;
        set
        {
            if (value < 0)
                throw new ArgumentException("Id cannot be less than 0", nameof(Id));
            _id = value;
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
        }
    }
    
    public Branch(int id, string location, string contactInfo)
    {
        Id = id;
        Location = location;
        ContactInfo = contactInfo;
        
        AddInstance(this);
    }
    
    public Branch() { }
    
    //--------------------------------------------
    public void AddEmployee(AbstractEmployee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (employee.EmployerBranch != null && employee.EmployerBranch != this)
        {
            throw new InvalidOperationException("Employee is already employed by another Branch");
        }
        //Problem with this method
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
    }
    
    internal void RemoveEmployeeInternal(AbstractEmployee employee)
    {
        _employees.Remove(employee);
    }
    
    public void AddManager(AbstractEmployee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (employee.ManagedBranch != null && employee.ManagedBranch != this)
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
    }
    
    internal void RemoveManagerInternal()
    {
        Manager = null;
    }
}