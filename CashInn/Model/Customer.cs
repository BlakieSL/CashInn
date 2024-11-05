using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Customer : ClassExtent<Customer>
{
    protected override string FilePath => ClassExtentFiles.CustomersFile;
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
}