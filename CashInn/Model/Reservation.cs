using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Reservation : ClassExtent<Reservation>
{
    protected override string FilePath => ClassExtentFiles.ReservationsFile;

    private readonly List<TableReservationAssociation> _tableAssociations = [];
    public IEnumerable<TableReservationAssociation> TableAssociations => _tableAssociations.AsReadOnly();
    
    public Customer Customer { get; private set; }
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
    
    public int NumberOfGuests 
    {
        get => _numberOfGuests;
        set
        {
            if (value < 0)
                throw new ArgumentException("NumberOfGuests cannot be less than 0", nameof(NumberOfGuests));
            _numberOfGuests = value;
        }
    }
    private int _numberOfGuests;

    public Reservation(int id, int numberOfGuests, Customer customer)
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        Id = id;
        NumberOfGuests = numberOfGuests;
        AddInstance(this);
        
        SetCustomer(customer);
    }
    
    public Reservation() { }
    
    public void SetCustomer(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        if (Customer == customer) return;

        if (Customer != null)
        {
            throw new InvalidOperationException("Reservation is already assigned to another Customer");
        }

        Customer = customer;
        customer.AddReservationInternal(this);
    }
    
    public void RemoveCustomer()
    { 
       RemoveInstance(this);
       Customer.RemoveReservationInternal(this);
    }

    public void AddTable(Table table, DateTime startTime, DateTime endTime)
    {
        ArgumentNullException.ThrowIfNull(table);

        var association = new TableReservationAssociation(table, this, startTime, endTime);
        _tableAssociations.Add(association);
        table.AddReservationAssociationInternal(association);
    }

    public void RemoveTable(Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        var association = _tableAssociations.FirstOrDefault(a => a.Table == table);
        if (association == null) return;

        _tableAssociations.Remove(association);
        table.RemoveReservationAssociationInternal(association);
    }

    internal void AddTableAssociation(TableReservationAssociation tableAssociation)
    {
        if (!_tableAssociations.Contains(tableAssociation))
        {
            _tableAssociations.Add(tableAssociation);
        }
    }

    internal void RemoveTableAssociation(TableReservationAssociation tableAssociation)
    {
        _tableAssociations.Remove(tableAssociation);
    }
}