using System.Text.Json;
using CashInn.Helper;

namespace CashInn.Model;

public class Branch
{
    private static readonly ICollection<Branch> Branches = new List<Branch>();
    
    public int Id { get; set; }
    public string Location { get; set; }
    public string ContactInfo { get; set; }
    public Employee Manager { get; set; }
    public ICollection<Employee> Employees { get; set; }
    public Menu Menu { get; set; }
    
    public Branch(int id, string location, string contactInfo, Employee manager,
        ICollection<Employee> employees, Menu menu)
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
        
        if (deserializedBranches != null)
        {
            foreach (var branch in deserializedBranches)
            {
                Branches.Add(branch);
            }
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