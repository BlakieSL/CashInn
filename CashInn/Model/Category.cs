using CashInn.Helper;
using CashInn.Model.Employee;
using CashInn.Model.MenuItem;

namespace CashInn.Model;

public class Category : ClassExtent<Category>
{
    protected override string FilePath => ClassExtentFiles.CategoriesFile;
    private readonly List<AbstractMenuItem> _menuItems = [];
    public IEnumerable<AbstractMenuItem> MenuItems => _menuItems.AsReadOnly();
    public readonly List<Menu> _menus = [];
    public IEnumerable<Menu> Menus => _menus.AsReadOnly();
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
    
    private string _name;
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

    public Category(int id, string name)
    {
        Name = name;
        Id = id;
        
        AddInstance(this);
    }
    
    public Category(){}

    public void AddMenuItem(AbstractMenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);

        if (menuItem.Category != null && menuItem.Category != this)
        {
            throw new ArgumentException("Menu item already belongs to another category", nameof(menuItem));
        }

        menuItem.SetCategory(this);
    }

    public void RemoveMenuItem(AbstractMenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);
        if (!_menuItems.Contains(menuItem))
        {
            throw new InvalidOperationException("MenuItem is not in this category");
        }

        menuItem.RemoveCategory();
    }

    internal void AddMenuItemInternal(AbstractMenuItem menuItem)
    {
        if(!_menuItems.Contains(menuItem))
            _menuItems.Add(menuItem);
    }

    internal void RemoveMenuItemInternal(AbstractMenuItem menuItem)
    {
            _menuItems.Remove(menuItem);
    }

    public void AddMenu(Menu menu)
    {
        ArgumentNullException.ThrowIfNull(menu);

        if (_menus.Contains(menu)) return;

        _menus.Add(menu);
        menu.AddCategoryInternal(this);
    }

    public void RemoveMenu(Menu menu)
    {
        ArgumentNullException.ThrowIfNull(menu);

        if (!_menus.Contains(menu)) return;

        _menus.Remove(menu);
        menu.RemoveCategoryInternal(this);
    }

    internal void AddMenuInternal(Menu menu)
    {
        if (!_menus.Contains(menu))
        {
            _menus.Add(menu);
        }
    }

    internal void RemoveMenuInternal(Menu menu)
    {
        _menus.Remove(menu);
    }
}