using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Kitchen : ClassExtent<Kitchen>
{
    public Chef? Manager { get; private set; }
    private readonly List<Cook> _cooks;
    public IEnumerable<Cook> Cooks => _cooks.AsReadOnly();
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
    
    
    
    public Kitchen(int id, List<string> equipment, List<Cook> cookList, Chef? manager = null)
    {
        Id = id;
        Equipment = equipment;
        AddManager(manager);
        if (cookList.Count == 0)
            throw new ArgumentException("At least one cook must be in cookList", nameof(cookList));
        _cooks = cookList;
        
        AddInstance(this);
    }
    
    public Kitchen(){}

    public void AddEquipment(string equipment)
    {
        if (string.IsNullOrWhiteSpace(equipment))
            throw new ArgumentException("Equipment must contain not be null or whitespace", nameof(equipment));
        Equipment.Add(equipment);
    }
    //--------------------------------------------
    public void AddManager(Chef manager)
    {
        ArgumentNullException.ThrowIfNull(manager);

        if (Manager == manager) return;

        if (Manager != null)
        {
            throw new InvalidOperationException("Cook is already managed by another Chef.");
        }

        Manager = manager;
        manager.AddKitchenInternal(this);
    }
    
    public void RemoveManager()
    {
        if (Manager == null) return;

        var currentManager = Manager;
        Manager = null;
        
        currentManager.RemoveKitchenInternal();
    }
    
    public void UpdateManager(Chef newManager)
    {
        ArgumentNullException.ThrowIfNull(newManager);

        RemoveManager();
        AddManager(newManager);
    }
    //--------------------------------------------
    public void AddCook(Cook cook)
    {
        ArgumentNullException.ThrowIfNull(cook);
        
        if (cook.Kitchen != null && cook.Kitchen != this)
        {
            throw new InvalidOperationException("Cook is already assigned to another kitchen");
        }
        
        cook.AddKitchen(this);
    }
    
    public void RemoveCook(Cook cook)
    {
        ArgumentNullException.ThrowIfNull(cook);
        if (!_cooks.Contains(cook)) return;

        cook.RemoveKitchen();
    }
    
    public void UpdateCook(Cook oldCook, Cook newCook)
    {
        ArgumentNullException.ThrowIfNull(oldCook);
        ArgumentNullException.ThrowIfNull(newCook);

        if(!_cooks.Contains(oldCook))
            throw new InvalidOperationException("Cook is not managed by this chef");

        AddCook(newCook);
        RemoveCook(oldCook);
    }

    internal void AddCookInternal(Cook cook)
    {
        if (!_cooks.Contains(cook))
        {
            _cooks.Add(cook);
        }
    }
    
    internal void RemoveCookInternal(Cook cook)
    {
        if (_cooks.Count <= 1)
        {
            throw new InvalidOperationException("There has to be at least one cook in the kitchen.");
        }
        _cooks.Remove(cook);
    }
}