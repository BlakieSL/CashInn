using CashInn.Helper;
using CashInn.Model.Employee;
using CashInn.Model.MenuItem;

namespace CashInn.Model;

public class Ingredient : ClassExtent<Ingredient>
{
    protected override string FilePath => ClassExtentFiles.IngredientsFile;
    private readonly List<AbstractMenuItem> _menuItems = [];
    public IEnumerable<AbstractMenuItem> MenuItems => _menuItems.AsReadOnly();
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

    public int Calories
    {
        get => _calories;
        set
        {
            if (value < 0)
                throw new ArgumentException("Id cannot be less than 0", nameof(Id));
            _calories = value;
        }
    }
    private int _calories;
    
    public bool IsInStock { get; set; }

    public Ingredient(int id, string name, int calories, bool isInStock)
    {
        Id = id;
        Name = name;
        Calories = calories;
        IsInStock = isInStock;
        
        AddInstance(this);
    }
    
    public Ingredient(){}

    public void AddMenuItem(AbstractMenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);

        if (_menuItems.Contains(menuItem)) return;

        _menuItems.Add(menuItem);
        menuItem.AddIngredientInternal(this);
    }

    public void RemoveMenuItem(AbstractMenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);
        if (!_menuItems.Contains(menuItem)) return;

        _menuItems.Remove(menuItem);
        menuItem.RemoveIngredientInternal(this);
    }

    public void AddMenuItemInternal(AbstractMenuItem menuItem)
    {
        if(!_menuItems.Contains(menuItem))
            _menuItems.Add(menuItem);
    }

    public void RemoveMenuItemInternal(AbstractMenuItem menuItem)
    {
        _menuItems.Remove(menuItem);
    }
}