using System.Text.Json.Serialization;
using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Customer : ClassExtent<Customer>
{
    protected override string FilePath => ClassExtentFiles.CustomersFile;

    private readonly List<Reservation> _reservations = [];
    [JsonIgnore] public IEnumerable<Reservation> Reservations => _reservations.AsReadOnly();

    private readonly List<Review> _reviews = [];
    [JsonIgnore] public IEnumerable<Review> Reviews => _reviews.AsReadOnly();
    
    private readonly List<Order> _orders = [];
    [JsonIgnore] public IEnumerable<Order> Orders => _orders.AsReadOnly();

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
    
    public string ContactNumber
    {
        get => _contactNumber;
        set
        {
            if (value != null && string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Contact number can be null but not empty", nameof(ContactNumber));
            _contactNumber = value;
        }
    }
    private string _contactNumber;
    
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

    public Customer(int id, string name, string? contactNumber, string address, string email)
    {
        Id = id;
        Name = name;
        ContactNumber = contactNumber;
        Address = address;
        Email = email;
        
        AddInstance(this);
    }
    
    public Customer(){}
    
    public void AddReservation(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        if (reservation.Customer != null && !Equals(reservation.Customer, this))
        {
            throw new InvalidOperationException("Employee is already employed by another Branch");
        }

        reservation.SetCustomer(this);
    }

    internal void AddReservationInternal(Reservation reservation)
    {
        if (!_reservations.Contains(reservation))
        {
            _reservations.Add(reservation);
        }

        UpdateInstance(this);
    }
    
    public void RemoveReservation(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        if (!_reservations.Contains(reservation))
        {
            throw new InvalidOperationException("Reservation is not made this Customer");
        }

        _reservations.Remove(reservation);
        UpdateInstance(this);
    }

    public void AddReview(Review review)
    {
        ArgumentNullException.ThrowIfNull(review);

        if (review.Customer != null && !Equals(review.Customer, this))
        {
            throw new InvalidOperationException("Review is already made by another Customer");
        }

        review.SetCustomer(this);
    }

    public void RemoveReview(Review review)
    {
        ArgumentNullException.ThrowIfNull(review);

        if (!_reviews.Contains(review))
        {
            throw new InvalidOperationException("Review is not made by this Customer");
        }
        
        _reviews.Remove(review);
        UpdateInstance(this);
    }

    internal void AddReviewInternal(Review review)
    {
        if(!_reviews.Contains(review))
            _reviews.Add(review);
        UpdateInstance(this);
    }
    
    public void AddOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (_orders.Contains(order)) return;
        
        if (order.Customer != null && !Equals(order.Customer, this))
        {
            throw new InvalidOperationException("Order is already assigned to another Customer");
        }

        order.SetCustomer(this);
    }
    
    internal void AddOrderInternal(Order order)
    {
        if(!_orders.Contains(order))
            _orders.Add(order);
        UpdateInstance(this);
    }

    protected internal override void RemoveInstance(Customer instance)
    {
        foreach (var reservation in _reservations)
        {
            reservation.RemoveInstance(reservation);
        }

        base.RemoveInstance(instance);
    }
}