using System.Text.Json.Serialization;
using CashInn.Helper;
using CashInn.Model.Employee;

namespace CashInn.Model;

public class Menu : ClassExtent<Menu>
{
    protected override string FilePath => ClassExtentFiles.MenusFile;
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
    public Menu(int id, DateTime dateUpdated)
    {
        Id = id;
        DateUpdated = dateUpdated;
        
        AddInstance(this);
    }
    
    public Menu(){}
}