using CashInn.Helper;

namespace CashInn.Model;

public class Table
{
    private static readonly ICollection<Table> Tables = new List<Table>();
    
    public int Id { get; set; }
    public int Capacity { get; set; }

    public Table()
    {
        
    }

    public Table(int id, int capacity)
    {
        Id = id;
        Capacity = capacity;
    }
    
    public static void SaveExtent(string filePath)
    {
        Saver.Serialize(Tables, filePath);
    }
    
    public static void LoadExtent(string filePath)
    {
        var deserializedTables = Saver.Deserialize<List<Table>>(filePath);
        Tables.Clear();

        if (deserializedTables == null) return;

        foreach (var table in deserializedTables)
        {
            Tables.Add(table);
        }
    }
    
    public static void SaveTable(Table table)
    {
        ArgumentNullException.ThrowIfNull(table);
        Tables.Add(table);
    }
    
    public static ICollection<Table> GetAllTables()
    {
        return Tables.ToList();
    }
}