using System.Text.Json.Serialization;
using CashInn.Helper;

namespace CashInn.Model;

public class MenuItem
{
    private static readonly ICollection<MenuItem> MenuItems = new List<MenuItem>();
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
    public double Price { get; set; }
    public string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Description cannot be null or empty", nameof(Description));
            _description = value;
        }
    }
    private string _description;
    public string DietaryInformation
    {
        get => _dietaryInformation;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("DietaryInformation", nameof(DietaryInformation));
            _dietaryInformation = value;
        }
    }
    private string _dietaryInformation;
    
    public bool Available { get; set; }
    [JsonIgnore]
    public ICollection<Menu> Menus;

    public MenuItem()
    {
        
    }

    public MenuItem(int id, string name, double price, string description, string dietaryInformation, bool available)
    {
        Id = id;
        Name = name;
        Price = price;
        Description = description;
        DietaryInformation = dietaryInformation;
        Available = available;
    }
    
    public static void SaveExtent(string filePath)
    {
        Saver.Serialize(MenuItems, filePath);
    }

    public static void LoadExtent(string filePath)
    {
        var deserializedMenuItems = Saver.Deserialize<List<MenuItem>>(filePath);
        MenuItems.Clear();

        if (deserializedMenuItems == null) return;
        foreach (var menuItem in deserializedMenuItems)
        {
            MenuItems.Add(menuItem);
        }
    }

    public static void SaveMenuItem(MenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);
        MenuItems.Add(menuItem);
    }

    public static ICollection<MenuItem> GetAllMenuItems()
    {
        return MenuItems.ToList();
    }
}