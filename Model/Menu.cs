namespace CashInn.Model;

public class Menu
{
    public int Id { get; set; }
    public ICollection<string> Categories { get; set; }
    public DateTime DateUpdated { get; set; }

    public ICollection<Branch> Branch { get; set; }
    public Dictionary<string, ICollection<MenuItem>> MenuItems { get; set; }
}