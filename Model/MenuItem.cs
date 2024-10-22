namespace CashInn.Model;

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string DietaryInformation { get; set; }
    public bool availible { get; set; }

    public ICollection<Menu> Menus;
}