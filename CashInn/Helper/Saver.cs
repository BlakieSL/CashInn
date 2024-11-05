using System.Text.Json;
using System.Text.Json.Serialization;

namespace CashInn.Helper;

public class Saver
{
    public static void Serialize<T>(T obj, string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() } 
            
        };

        string jsonString = JsonSerializer.Serialize(obj, options);
        File.WriteAllText(filePath, jsonString);
    }
    
    public static T? Deserialize<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found");
        }
        
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        
        string jsonString = File.ReadAllText(filePath);
        T? obj = default(T);
        try
        {
            obj = JsonSerializer.Deserialize<T>(jsonString, options);
            if (obj == null)
            {
                throw new InvalidOperationException("Error deserialization");
            }
        
            return obj;
        } catch (JsonException e)
        {
            Console.WriteLine("The file containing the objects is empty: " + e.Message);
        }
        return obj;
    }
}