using System.Collections.Immutable;
using System.Text.Json;
using CashInn.Enum;
using CashInn.Helper;

namespace CashInn.Model.MenuItem;
public abstract class AbstractMenuItem
{
    private static readonly List<AbstractMenuItem> MenuItems = [];
    private static string _filepath = ClassExtentFiles.MenuItemFile;

    private readonly List<AbstractMenuItem> _relatedItems = [];
    public IEnumerable<AbstractMenuItem> RelatedItems => _relatedItems.AsReadOnly();
    
    private readonly List<AbstractMenuItem> _referencingItems = [];
    public IEnumerable<AbstractMenuItem> ReferencingItems => _referencingItems.AsReadOnly();
    
    private readonly List<Ingredient> _ingredients = [];
    public IEnumerable<Ingredient> Ingredients => _ingredients.AsReadOnly();
    
    private readonly List<AbstractMenuItemOrderAssociation> _orderAssociations = [];
    public IEnumerable<AbstractMenuItemOrderAssociation> OrderAssociations => _orderAssociations.AsReadOnly();
    
    public Category Category { get; private set; }
    public abstract string ItemType { get; }
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

    private double _price;
    public double Price
    {
        get => _price;
        set
        {
            if (value < 0)
                throw new ArgumentException("Price cannot be negative", nameof(Price));
            _price = value;
        }
    }

    private string _description;
    public string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Description cannot be null or empty", nameof(Description));
            _description = value;
        }
    }

    private string _dietaryInformation;
    public string DietaryInformation
    {
        get => _dietaryInformation;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Dietary information cannot be null or empty", nameof(DietaryInformation));
            _dietaryInformation = value;
        }
    }

    public bool Available { get; set; }
    
    protected AbstractMenuItem(int id, string name, double price, string description, string dietaryInformation,
        bool available, Category category)
    {
        Id = id;
        Name = name;
        Price = price;
        Description = description;
        DietaryInformation = dietaryInformation;
        Available = available;
        
        AddCategory(category);
    }
    public static void SaveExtent()
    {
        Saver.Serialize(MenuItems.Select(e => e.ToSerializableObject()).ToList(), _filepath);
    }

    public abstract object ToSerializableObject();
    public static void LoadExtent()
    {
        var deserializedEmployees = Saver.Deserialize<List<dynamic>>(_filepath);
        MenuItems.Clear();

        if (deserializedEmployees == null) return;

        foreach (var itemData in deserializedEmployees)
        {
            int id = itemData.GetProperty("Id").GetInt32();
            string name = itemData.GetProperty("Name").GetString();
            double price = itemData.GetProperty("Price").GetDouble();
            string description = itemData.GetProperty("Description").GetString();
            string dietaryInformation = itemData.GetProperty("DietaryInformation").GetString();
            bool available = itemData.GetProperty("Available").GetBoolean();

            string itemType = itemData.GetProperty("ItemType").GetString();
            Category category = JsonSerializer.Deserialize<Category>(itemData.GetProperty("Category").GetRawText());
            if (category == null)
            {
                throw new InvalidOperationException("Category cannot be null.");
            }
            
            AbstractMenuItem abstractMenuItem;
            if (itemType == "Default")
            {
                var servingSizeString = itemData.GetProperty("ServingSize").GetString();
                ServingSize servingSize;
                if (System.Enum.TryParse<ServingSize>(servingSizeString, out ServingSize size))
                {
                    servingSize = size;
                } else
                {
                    throw new ArgumentException($"Invalid status value: {servingSizeString}");
                }
                
                abstractMenuItem = new DefaultItem(
                    id,
                    name,
                    price,
                    description,
                    dietaryInformation,
                    available,
                    servingSize,
                    category
                );
            }
            else if (itemType == "Special")
            {
                DateTime? validFrom = null;
                DateTime? validTo = null;
                if (itemData.TryGetProperty("ValidFrom", out JsonElement validFromProperty) && validFromProperty.ValueKind != JsonValueKind.Null)
                {
                    validFrom = validFromProperty.GetDateTime();
                }
                if (itemData.TryGetProperty("ValidTo", out JsonElement validToProperty) && validToProperty.ValueKind != JsonValueKind.Null)
                {
                    validTo = validToProperty.GetDateTime();
                }
                
                abstractMenuItem = new SpecialItem(
                    id,
                    name,
                    price,
                    description,
                    dietaryInformation,
                    available,
                    validFrom.Value,
                    validTo.Value,
                    category
                );
            } 
            else
            {
                throw new InvalidOperationException("Unknown employee type.");
            }
        }
    }
    
    public static void SaveItem(AbstractMenuItem abstractMenuItem)
    {
        ArgumentNullException.ThrowIfNull(abstractMenuItem);
        MenuItems.Add(abstractMenuItem);
    }
    
    public static ICollection<AbstractMenuItem> GetAll()
    {
        return MenuItems.ToImmutableList();
    }

    public void AddCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (Category == category) return;

        if (Category != null)
        {
            throw new InvalidOperationException("Menu item is already in another category.");
        }

        Category = category;
        category.AddMenuItemInternal(this);
    }

    public void RemoveCategory()
    {
        if (Category == null) return;

        var currentCategory = Category;
        Category = null;
        currentCategory.RemoveMenuItemInternal(this);
    }

    public void UpdateCategory(Category newCategory)
    {
        ArgumentNullException.ThrowIfNull(newCategory);

        RemoveCategory();
        AddCategory(newCategory);
    }

    public void AddIngredient(Ingredient ingredient)
    {
        ArgumentNullException.ThrowIfNull(ingredient);

        if (_ingredients.Contains(ingredient)) return;

        _ingredients.Add(ingredient);
        ingredient.AddMenuItemInternal(this);
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        ArgumentNullException.ThrowIfNull(ingredient);

        if (!_ingredients.Contains(ingredient)) return;

        _ingredients.Remove(ingredient);
        ingredient.RemoveMenuItemInternal(this);
    }

    internal void AddIngredientInternal(Ingredient ingredient)
    {
        if(!_ingredients.Contains(ingredient))
            _ingredients.Add(ingredient);
    }

    internal void RemoveIngredientInternal(Ingredient ingredient)
    {
        _ingredients.Remove(ingredient);
    }

    public void AddRelatedItem(AbstractMenuItem relatedItem)
    {
        ArgumentNullException.ThrowIfNull(relatedItem);

        if (relatedItem == this)
            throw new InvalidOperationException("An item cannot be related to itself.");

        if (_relatedItems.Contains(relatedItem)) return;

        _relatedItems.Add(relatedItem);
        relatedItem.AddReferencingItemInternal(this);
    }

    public void RemoveRelatedItem(AbstractMenuItem relatedItem)
    {
        ArgumentNullException.ThrowIfNull(relatedItem);

        if (!_relatedItems.Contains(relatedItem)) return;
        _relatedItems.Remove(relatedItem);

        relatedItem.RemoveReferencingItemInternal(this);
    }

    internal void AddRelatedItemInternal(AbstractMenuItem relatedItem)
    {
        if (!_relatedItems.Contains(relatedItem))
        {
            _relatedItems.Add(relatedItem);
        }
    }

    internal void RemoveRelatedItemInternal(AbstractMenuItem relatedItem)
    {
        _relatedItems.Remove(relatedItem);
    }

    public void AddReferencingItem(AbstractMenuItem referencingItem)
    {
        ArgumentNullException.ThrowIfNull(referencingItem);
        if (referencingItem == this) throw new InvalidOperationException("An item cannot reference itself.");

        if (_referencingItems.Contains(referencingItem)) return;

        AddReferencingItemInternal(referencingItem);
        referencingItem.AddRelatedItemInternal(this);
    }

    public void RemoveReferencingItem(AbstractMenuItem referencingItem)
    {
        ArgumentNullException.ThrowIfNull(referencingItem);

        if (!_referencingItems.Contains(referencingItem)) return;

        RemoveReferencingItemInternal(referencingItem);
        referencingItem.RemoveRelatedItemInternal(this);
    }

    internal void AddReferencingItemInternal(AbstractMenuItem referencingItem)
    {
        if (!_referencingItems.Contains(referencingItem))
            _referencingItems.Add(referencingItem);
    }

    internal void RemoveReferencingItemInternal(AbstractMenuItem referencingItem)
    {
        _referencingItems.Remove(referencingItem);
    }

    public void AddOrder(Order order, int quantity)
    {
        ArgumentNullException.ThrowIfNull(order);

        var association = new AbstractMenuItemOrderAssociation(this, order, quantity);
        _orderAssociations.Add(association);
        order.AddMenuItemAssociationInternal(association);
    }

    public void RemoveOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order); 
        
        var association = _orderAssociations.FirstOrDefault(a => a.order == order);
        if (association == null) return;

        _orderAssociations.Remove(association);
        order.RemoveMenuItemAssociationInternal(association);
    }

    internal void AddOrderAssociationInternal(AbstractMenuItemOrderAssociation association)
    {
        if(!_orderAssociations.Contains(association))
            _orderAssociations.Add(association);
    }

    internal void RemoveOrderAssociationInternal(AbstractMenuItemOrderAssociation association)
    {
        _orderAssociations.Remove(association);
    }
}