using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Table : ClassExtent<Table>
{
    protected override string FilePath => ClassExtentFiles.TablesFile;

    private readonly List<TableReservationAssociation> _reservationAssociations = [];
    public IEnumerable<TableReservationAssociation> ReservationAssociations => _reservationAssociations.AsReadOnly();
    
    public Waiter? Waiter { get; private set; }
    public Branch Branch { get; private set; }
    public int TableNumber 
    {
        get => _tableNumber;
        set
        {
            if (value < 0)
                throw new ArgumentException("TableNumber cannot be less than 0", nameof(TableNumber));
            _tableNumber = value;
        }
    }
    private int _tableNumber;

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
    
    public Table(int tableNumber, int capacity, Branch branch)
    {
        if (branch == null) throw new ArgumentNullException(nameof(branch));
        TableNumber = tableNumber;
        Capacity = capacity;
        AddInstance(this);
        
        SetBranch(branch);
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
    
    public void SetBranch(Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);

        if (Branch == branch) return;

        if (Branch != null)
        {
            throw new InvalidOperationException("Table is already assigned to another Waiter.");
        }
        
        Branch = branch;
        
        Branch.AddTableInternal(this);
    }
    
    public void RemoveBranch()
    {
        RemoveInstance(this);
        Branch.RemoveTableInternal(this);
    }

    public void AddReservation(Reservation reservation, DateTime startTime, DateTime endTime)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        var association = new TableReservationAssociation(this, reservation, startTime, endTime);
        _reservationAssociations.Add(association);
        reservation.AddTableAssociation(association);
    }

    public void RemoveReservation(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        var association = _reservationAssociations.FirstOrDefault(a => a.Reservation == reservation);
        if (association == null) return;

        _reservationAssociations.Remove(association);
        reservation.RemoveTableAssociation(association);
    }

    internal void AddReservationAssociationInternal(TableReservationAssociation association)
    {
        if(!_reservationAssociations.Contains(association)) 
            _reservationAssociations.Add(association);
    }

    internal void RemoveReservationAssociationInternal(TableReservationAssociation association)
    {
        _reservationAssociations.Remove(association);
    }
}