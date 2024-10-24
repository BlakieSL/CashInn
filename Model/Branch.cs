using System.Text.Json.Serialization;
using CashInn.Helper;

namespace CashInn.Model;

public class Branch
{
    private static readonly ICollection<Branch> Branches = new List<Branch>();
    public int Id { get; set; }
    
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
    
    [JsonIgnore]
    public AbstractEmployee? Manager { get; set; }
    [JsonIgnore]
    public ICollection<AbstractEmployee> Employees { get; set; }
    [JsonIgnore]
    public Menu Menu { get; set; }

    public Branch()
    {
        
    }

    public Branch(int id, string location, string contactInfo)
    {
        Id = id;
        Location = location;
        ContactInfo = contactInfo;
    }
    
    public Branch(int id, string location, string contactInfo, ICollection<AbstractEmployee> employees, 
        Menu menu, AbstractEmployee? manager = null)
    {
        Id = id;
        Location = location;
        ContactInfo = contactInfo;
        Manager = manager;
        Employees = employees;
        Menu = menu;
    }

    
    public static void SaveExtent(string filePath)
    {
        Saver.Serialize(Branches, filePath);
    }

    public static void LoadExtent(string filePath)
    {
        var deserializedBranches = Saver.Deserialize<List<Branch>>(filePath);
        Branches.Clear();

        if (deserializedBranches == null) return;
        foreach (var branch in deserializedBranches)
        {
            Branches.Add(branch);
        }
    }

    public static void SaveBranch(Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);
        Branches.Add(branch);
    }

    public static ICollection<Branch> GetAllBranches()
    {
        return Branches.ToList();
    }
}