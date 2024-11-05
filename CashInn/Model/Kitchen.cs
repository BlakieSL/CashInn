using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Kitchen : ClassExtent<Kitchen>
{
    protected override string FilePath => ClassExtentFiles.KitchensFile;
    public int Id 
    {
        get => _id;
        set
        {
            if (value < 0)
                throw new ArgumentException("Id cannot be less than 0", nameof(Id));
            _id = value;
        }
    }
    private int _id;
    
    public List<string> Equipment
    {
        get => _equipment;
        set
        {
            if (value == null || value.Count == 0)
                throw new ArgumentException("Equipment list must contain at least one item", nameof(Equipment));
            if (value.Any(string.IsNullOrWhiteSpace)) 
                throw new ArgumentException("Equipment list must not contain null or whitespace elements", nameof(Equipment));
            _equipment = value;
        }
    }
    private List<string> _equipment;
    
    public Kitchen(int id, List<string> equipment)
    {
        Id = id;
        Equipment = equipment;
        
        AddInstance(this);
    }
    
    public Kitchen(){}

    public void AddEquipment(string equipment)
    {
        if (string.IsNullOrWhiteSpace(equipment))
            throw new ArgumentException("Equipment must contain not be null or whitespace", nameof(equipment));
        Equipment.Add(equipment);
    }
}