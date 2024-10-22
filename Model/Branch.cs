using System.Text.Json;

namespace CashInn.Model;

public class Branch
{
    public Branch(int id, string location, string contactInfo, Employee manager, ICollection<Employee> employees, Menu menu)
    {
        Id = id;
        Location = location;
        ContactInfo = contactInfo;
        Manager = manager;
        Employees = employees;
        Menu = menu;
    }

    public int Id { get; set; }
    public string Location { get; set; }
    public string ContactInfo { get; set; }

    public Employee Manager { get; set; }
    public ICollection<Employee> Employees { get; set; }
    public Menu Menu { get; set; }

    public static ICollection<Branch> AllInstances { get; set; }
    
    public static void SaveExtent(string filePath)
    {
        var json = JsonSerializer.Serialize(AllInstances);
        File.WriteAllText(filePath, json);
    }

    public static void LoadExtent(string filePath)
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            AllInstances = JsonSerializer.Deserialize<List<Branch>>(json);
        }
    }
}