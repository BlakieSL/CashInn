using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Category : ClassExtent<Category>
{
    protected override string FilePath => ClassExtentFiles.CategoriesFile;
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
}