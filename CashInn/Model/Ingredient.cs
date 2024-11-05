using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Ingredient : ClassExtent<Ingredient>
{
    protected override string FilePath => ClassExtentFiles.IngredientsFile;
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
}