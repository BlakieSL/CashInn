namespace CashInn.Model;

public class TableReservationAssociation
{
    public Table Table { get; private set; }
    public Reservation Reservation { get; private set; }
    
    private DateTime _startDateTime;
    public DateTime StartDateTime
    {
        get => _startDateTime;
        set
        {
            if (value > EndDateTime)
            {
                throw new ArgumentException("StartDateTime cannot be after EndDateTime", nameof(StartDateTime));
            }
            _startDateTime = value;
        }
    }

    private DateTime _endDateTime;
    public DateTime EndDateTime
    {
        get => _endDateTime;
        set
        {
            if (value < StartDateTime)
            {
                throw new ArgumentException("EndDateTime cannot be before StartDateTime", nameof(EndDateTime));
            }
            _endDateTime = value;
        }
    }

    public TableReservationAssociation(Table table, Reservation reservation, DateTime startDateTime,
        DateTime endDateTime)
    {
        if (startDateTime > endDateTime)
        {
            throw new ArgumentException("StartDateTime cannot be after EndDateTime", nameof(startDateTime));
        }
        _startDateTime = startDateTime;
        _endDateTime = endDateTime;
        Table = table;
        Reservation = reservation;
    }

    public void UpdateStartDateTime(DateTime newStartDateTime)
    {
        StartDateTime = newStartDateTime;
    }

    public void UpdateEndDateTime(DateTime newEndDateTime)
    {
        EndDateTime = newEndDateTime;
    }
}