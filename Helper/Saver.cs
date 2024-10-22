using System.Text.Json;

namespace CashInn.Helper;

public class Saver
{
    public void Serialize<T>(T obj, string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string jsonString = JsonSerializer.Serialize(obj, options);
        File.WriteAllText(filePath, jsonString);
    }
    
    public T? Deserialize<T>(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        
        T? obj = JsonSerializer.Deserialize<T>(jsonString);

        if (obj == null)
        {
            throw new InvalidOperationException("Error deserialization");
        }
        
        return obj;
    }
}