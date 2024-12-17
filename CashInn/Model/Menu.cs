using System.Text.Json.Serialization;
using CashInn.Helper;

namespace CashInn.Model;

public class Menu : ClassExtent<Menu>
{
    protected override string FilePath => ClassExtentFiles.MenusFile;
    private readonly List<Category> _categories = [];
    public IEnumerable<Category> Categories => _categories.AsReadOnly();
    public Branch Branch { get; private set; }
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

    private DateTime _dateUpdated;
    public DateTime DateUpdated
    {
        get => _dateUpdated;
        set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("DateUpdated cannot be in the future", nameof(DateUpdated));
            _dateUpdated = value;
        }
    }
    public Menu(int id, DateTime dateUpdated, Branch? branch = null)
    {
        Id = id;
        DateUpdated = dateUpdated;

        AddInstance(this);
        
        if (branch != null) 
            SetBranch(branch);
    }

    public Menu(){}

    public void AddCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (_categories.Contains(category)) return;

        _categories.Add(category);
        category.AddMenuInternal(this);
    }

    public void RemoveCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if(!_categories.Contains(category)) return;

        if (_categories.Count <= 1)
            throw new InvalidOperationException("Size of Categories cannot be less than 1");

        _categories.Remove(category);
        category.RemoveMenuInternal(this);
    }

    internal void AddCategoryInternal(Category category)
    {
        if (!_categories.Contains(category))
        {
            _categories.Add(category);
        }
    }

    internal void RemoveCategoryInternal(Category category)
    {
        _categories.Remove(category);
    }
    
    public void SetBranch(Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);
        if (Branch == branch) return;

        if (Branch != null)
        {
            throw new InvalidOperationException("Menu is already assigned to another Branch");
        }

        Branch = branch;
        branch.AddMenuInternal(this);
    }
}