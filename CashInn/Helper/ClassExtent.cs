using System.Collections.ObjectModel;
using CashInn.Model.Employee;
using CashInn.Model.Payment;

namespace CashInn.Helper;

public abstract class ClassExtent<T> where T : ClassExtent<T>
{
    private static readonly List<T> Instances = [];
    protected abstract string FilePath { get; }
    public static void SaveExtent()
    {
        Saver.Serialize(Instances.ToList(), GetFilePath());
    }
    
    public static IReadOnlyCollection<T> GetAll()
    {
        return new ReadOnlyCollection<T>(Instances.ToList());
    }

    protected static void AddInstance(T instance)
    {
        Instances.Add(instance);
    }

    public static void LoadExtent()
    {
        if (!File.Exists(GetFilePath())) return;

        var deserializedData = Saver.Deserialize<List<T>>(GetFilePath());
        Instances.Clear();

        if (deserializedData != null)
        {
            foreach (var item in deserializedData)
            {
                Instances.Add(item);
            }
        }
    }
    
    private static string GetFilePath()
    {
        var instance = (T)Activator.CreateInstance(typeof(T), true)!; 
        return instance.FilePath;
    }
}