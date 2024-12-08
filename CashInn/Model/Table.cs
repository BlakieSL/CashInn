using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Table : ClassExtent<Table>
{
    public Waiter? Waiter { get; private set; }
    protected override string FilePath => ClassExtentFiles.TablesFile;
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

    private int _capacity;
    public int Capacity
    {
        get => _capacity;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Capacity must be greater than zero", nameof(Capacity));
            _capacity = value;
        }
    }
    
    public Table(int id, int capacity)
    {
        Id = id;
        Capacity = capacity;
        
        AddInstance(this);
    }
    
    public Table(){}
    
    public void AddWaiter(Waiter waiter)
    {
        ArgumentNullException.ThrowIfNull(waiter);

        if (Waiter == waiter) return;

        if (Waiter != null)
        {
            throw new InvalidOperationException("Table is already assigned to another Waiter.");
        }

        Waiter = waiter;
        waiter.AddTableInternal(this);
    }

    public void RemoveWaiter()
    {
        if (Waiter == null) return;

        var currentWaiter = Waiter;
        Waiter = null;
        currentWaiter.RemoveTableInternal(this);
    }

    public void UpdateWaiter(Waiter newWaiter)
    {
        ArgumentNullException.ThrowIfNull(newWaiter);

        RemoveWaiter();
        AddWaiter(newWaiter);
    }
}