using System.Collections.ObjectModel;

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
        if (instance != null!) Instances.Add(instance);
    }
    
    protected static void RemoveInstance(T instance)
    {
        if (instance != null!) Instances.Remove(instance);
    }
    
    public static void UpdateInstance(T instance)
    {
        if (instance == null) throw new ArgumentNullException(nameof(instance));

        var index = Instances.IndexOf(instance);
        if (index >= 0)
        {
            Instances[index] = instance;
        }
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

    public static void ClearExtent()
    {
        Instances.Clear();
    }
}