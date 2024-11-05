using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Reservation : ClassExtent<Reservation>
{
    protected override string FilePath => ClassExtentFiles.ReservationsFile;
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

    public Reservation(int id, int numberOfGuests)
    {
        Id = id;
        NumberOfGuests = numberOfGuests;
        
        AddInstance(this);
    }
    
    public Reservation(){}
}