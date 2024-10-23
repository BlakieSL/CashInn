using System.Text.Json.Serialization;
using CashInn.Helper;

namespace CashInn.Model;

public class Menu
{
    private static readonly ICollection<Menu> Menus = new List<Menu>();
    public int Id { get; set; }
    public DateTime DateUpdated { get; set; }
    [JsonIgnore]
    public ICollection<string> Categories { get; set; }
    [JsonIgnore]
    public ICollection<Branch> Branch { get; set; }
    [JsonIgnore]
    public Dictionary<string, ICollection<MenuItem>> MenuItems { get; set; }

    public Menu()
    {
        
    }

    public Menu(int id, DateTime dateUpdated)
    {
        Id = id;
        DateUpdated = dateUpdated;
    }
    
    public static void SaveExtent(string filePath)
    {
        Saver.Serialize(Menus, filePath);
    }
    
    public static void LoadExtent(string filePath)
    {
        var deserializedMenus = Saver.Deserialize<List<Menu>>(filePath);
        Menus.Clear();
            
        if (deserializedMenus == null) return;
        foreach (var menu in deserializedMenus)
        {
            Menus.Add(menu);
        }
    }
    
    public static void SaveMenu(Menu menu)
    {
        ArgumentNullException.ThrowIfNull(menu);
        Menus.Add(menu);
    }
    
    public static ICollection<Menu> GetAllMenus()
    {
        return Menus.ToList();
    }
}