using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Branch : ClassExtent<Branch>
{
    protected override string FilePath => ClassExtentFiles.BranchesFile;
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
    
    private string _location;
    public string Location
    {
        get => _location;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Location cannot be null or empty", nameof(Location));
            _location = value;
        }
    }

    private string _contactInfo;
    public string ContactInfo
    {
        get => _contactInfo;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Contact info cannot be null or empty", nameof(ContactInfo));
            _contactInfo = value;
        }
    }
    
    public Branch(int id, string location, string contactInfo)
    {
        Id = id;
        Location = location;
        ContactInfo = contactInfo;
        
        AddInstance(this);
    }
    
    public Branch() { }
}