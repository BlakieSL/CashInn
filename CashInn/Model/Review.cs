using CashInn.Enum;
using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Review : ClassExtent<Review>
{
    protected override string FilePath => ClassExtentFiles.ReviewsFile;
    public Customer Customer { get; private set; }
    public Order Order { get; private set; }
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

    public Rating ReviewRating { get; set; }
    public string Description 
    {
        get => _description;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Description cannot be null or empty");
            _description = value;
        }
    }
    private string _description;

    public Review(int id, Rating rating, string description, Customer? customer = null, Order? order = null)
    {
        Id = id;
        ReviewRating = rating;
        Description = description;

        AddInstance(this);
        if (customer != null)
            SetCustomer(customer);
        if (order != null)
            SetOrder(order);
    }
    
    public Review(){}

    public void SetCustomer(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        
        if (Customer == customer) return;

        if (Customer != null)
            throw new InvalidOperationException("Review is already assigned to a Customer");

        if (customer.Reviews.Contains(this)) return;
        
        Customer = customer;
        customer.AddReviewInternal(this);
    }

    public void SetOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (order == Order) return;

        if (Order != null)
            throw new InvalidOperationException("Review is already assigned to another Order");

        if (order.Review != null)
            throw new InvalidOperationException("Order already has another Review");

        Order = order;
        order.SetReviewInternal(this);
    }
}