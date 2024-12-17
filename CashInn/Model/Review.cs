using CashInn.Enum;
using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Review : ClassExtent<Review>
{
    protected override string FilePath => ClassExtentFiles.ReviewsFile;

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

    public Review(int id, Rating rating, string description, Customer customer)
    {
        Id = id;
        ReviewRating = rating;
        Description = description;
        SetCustomer(customer);

        AddInstance(this);
    }
    
    public Review(){}

    public void SetCustomer(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        if (Customer == customer) return;

        Customer?.RemoveReviewInternal(this);

        Customer = customer;
        customer.AddReviewInternal(this);
    }

    public void RemoveCustomer()
    {
        Customer.RemoveReviewInternal(this);
    }

}