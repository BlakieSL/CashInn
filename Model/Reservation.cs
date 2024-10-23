using CashInn.Helper;

namespace CashInn.Model;

public class Reservation
{
    private static readonly ICollection<Reservation> Reservations = new List<Reservation>();

    public int Id { get; set; }
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be null or empty", nameof(Name));
            _name = value;
        }
    }
    private string _name;
    public string? ContactNumber { get; set; }
    public string Address
    {
        get => _address;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Address info cannot be null or empty");
            _address = value;
        }
    }
    private string _address;

    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be null or empty");
            _email = value;
        }
    }

    private string _email;

    //add customer reserver
    
    public Reservation()
    {
        
    }

    public Reservation(int id, string name, string address, string email, string? contactNumber = null)
    {
        Id = id;
        Name = name;
        Address = address;
        Email = email;
        ContactNumber = ContactNumber;
    }
    
    public static void SaveExtent(string filePath)
    {
        Saver.Serialize(Reservations, filePath);
    }
    
    public static void LoadExtent(string filePath)
    {
        var deserializedReservations = Saver.Deserialize<List<Reservation>>(filePath);
        Reservations.Clear();

        if (deserializedReservations == null) return;

        foreach (var reservation in deserializedReservations)
        {
            Reservations.Add(reservation);
        }
    }
    
    public static void SaveReservation(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);
        Reservations.Add(reservation);
    }
    
    public static ICollection<Reservation> GetAllReservations()
    {
        return Reservations.ToList();
    }
}