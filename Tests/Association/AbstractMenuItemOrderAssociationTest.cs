using CashInn.Model;
using CashInn.Model.MenuItem;
using CashInn.Enum;
using NUnit.Framework;

namespace Tests.Association;

[TestFixture]
[TestOf(typeof(AbstractMenuItemOrderAssociation))]
public class MenuItemOrderAssociationTests
{
    private Order _order1 = null!;
    private Order _order2 = null!;
    private DefaultItem _menuItem1 = null!;
    private DefaultItem _menuItem2 = null!;
    private Category _category = null!;

    [SetUp]
    public void SetUp()
    {
        _category = new Category(1, "Main Dishes");

        _menuItem1 = new DefaultItem(1, "Burger", 10.99, "Beef Burger", 
            "High Protein", true, ServingSize.Small, _category);
        _menuItem2 = new DefaultItem(2, "Fries", 4.99, "Crispy Fries", 
            "Vegetarian", true, ServingSize.Medium, _category);

        _order1 = new Order(1, DateTime.Today, false);
        _order2 = new Order(2, DateTime.Today, true);
    }

    [Test]
    public void AddMenuItemToOrder_ShouldCreateBidirectionalAssociation()
    {
        _order1.AddMenuItem(_menuItem1, 2);
        
        Assert.Multiple(() =>
        {
            Assert.That(_order1.MenuItemAssociations.Count(), Is.EqualTo(1));
            Assert.That(_menuItem1.OrderAssociations.Count(), Is.EqualTo(1));
            Assert.That(_order1.MenuItemAssociations.First().MenuItem, Is.EqualTo(_menuItem1));
            Assert.That(_menuItem1.OrderAssociations.First().Order, Is.EqualTo(_order1));
            Assert.That(_order1.MenuItemAssociations.First().Quantity, Is.EqualTo(2));
        });
    }

    [Test]
    public void AddMenuItemToOrder_WhenSameMenuItemMultipleTimes_ShouldAllowMultipleEntries()
    {
        _order1.AddMenuItem(_menuItem1, 2);
        _order1.AddMenuItem(_menuItem1, 3);

        Assert.That(_order1.MenuItemAssociations.Count(), Is.EqualTo(2));
        Assert.That(_order1.MenuItemAssociations.ElementAt(0).Quantity, Is.EqualTo(2));
        Assert.That(_order1.MenuItemAssociations.ElementAt(1).Quantity, Is.EqualTo(3));
    }

    [Test]
    public void AddMenuItemToOrder_WhenQuantityIsNegative_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _order1.AddMenuItem(_menuItem1, -1));
    }

    [Test]
    public void RemoveMenuItemFromOrder_ShouldRemoveBidirectionalAssociation()
    {
        _order1.AddMenuItem(_menuItem1, 2);
        _order1.RemoveMenuItem(_menuItem1);

        Assert.Multiple(() =>
        {
            Assert.That(_order1.MenuItemAssociations, Has.None.Matches<AbstractMenuItemOrderAssociation>(
                a => a.MenuItem == _menuItem1));
            Assert.That(_menuItem1.OrderAssociations, Has.None.Matches<AbstractMenuItemOrderAssociation>(
                a => a.Order == _order1));
        });

    }

    [Test]
    public void RemoveMenuItemFromOrder_WhenMenuItemNotAssociated_ShouldNotThrowException()
    {
        Assert.DoesNotThrow(() => _order1.RemoveMenuItem(_menuItem1));
    }

    [Test]
    public void AddOrderToMenuItem_ShouldCreateBidirectionalAssociation()
    {
        _menuItem1.AddOrder(_order1, 5);

        Assert.Multiple(() =>
        {
            Assert.That(_menuItem1.OrderAssociations.Count(), Is.EqualTo(1));
            Assert.That(_order1.MenuItemAssociations.Count(), Is.EqualTo(1));
            Assert.That(_menuItem1.OrderAssociations.First().Order, Is.EqualTo(_order1));
            Assert.That(_order1.MenuItemAssociations.First().MenuItem, Is.EqualTo(_menuItem1));
            Assert.That(_menuItem1.OrderAssociations.First().Quantity, Is.EqualTo(5));
        });
    }

    [Test]
    public void RemoveOrderFromMenuItem_ShouldRemoveBidirectionalAssociation()
    {
        _menuItem1.AddOrder(_order1, 3);
        _menuItem1.RemoveOrder(_order1);

        Assert.Multiple(() =>
        {
            Assert.That(_menuItem1.OrderAssociations, Has.None.Matches<AbstractMenuItemOrderAssociation>(
                a => a.Order == _order1));

            Assert.That(_order1.MenuItemAssociations, Has.None.Matches<AbstractMenuItemOrderAssociation>(
                a => a.MenuItem == _menuItem1));
        });

    }

    [Test]
    public void AddMultipleMenuItemsToOrder_ShouldCreateMultipleAssociations()
    {
        _order1.AddMenuItem(_menuItem1, 2);
        _order1.AddMenuItem(_menuItem2, 4);

        Assert.Multiple(() =>
        {
            Assert.That(_order1.MenuItemAssociations.Count(), Is.EqualTo(2));
            Assert.That(_menuItem1.OrderAssociations.Count(), Is.EqualTo(1));
            Assert.That(_menuItem2.OrderAssociations.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public void AddMultipleOrdersToMenuItem_ShouldCreateMultipleAssociations()
    {
        _menuItem1.AddOrder(_order1, 2);
        _menuItem1.AddOrder(_order2, 3);

        Assert.Multiple(() =>
        {
            Assert.That(_menuItem1.OrderAssociations.Count(), Is.EqualTo(2));
            Assert.That(_order1.MenuItemAssociations.Count(), Is.EqualTo(1));
            Assert.That(_order2.MenuItemAssociations.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public void RemoveMenuItemFromOrder_ShouldNotAffectOtherAssociations()
    {
        _order1.AddMenuItem(_menuItem1, 2);
        _order1.AddMenuItem(_menuItem2, 3);

        _order1.RemoveMenuItem(_menuItem1);

        Assert.Multiple(() =>
        {
            Assert.That(_order1.MenuItemAssociations, Contains.Item(_order1.MenuItemAssociations.FirstOrDefault(a => a.MenuItem == _menuItem2)));
            Assert.That(_menuItem2.OrderAssociations, Contains.Item(_menuItem2.OrderAssociations.FirstOrDefault(a => a.Order == _order1)));
            Assert.That(_menuItem1.OrderAssociations, Is.Empty);
        });
    }

    [Test]
    public void RemoveOrderFromMenuItem_ShouldNotAffectOtherAssociations()
    {
        _menuItem1.AddOrder(_order1, 2);
        _menuItem1.AddOrder(_order2, 3);

        _menuItem1.RemoveOrder(_order1);

        Assert.Multiple(() =>
        {
            Assert.That(_menuItem1.OrderAssociations, Contains.Item(_menuItem1.OrderAssociations.FirstOrDefault(a => a.Order == _order2)));
            Assert.That(_order2.MenuItemAssociations, Contains.Item(_order2.MenuItemAssociations.FirstOrDefault(a => a.MenuItem == _menuItem1)));
            Assert.That(_order1.MenuItemAssociations, Is.Empty);
        });
    }
    
    [Test]
    public void UpdateQuantity_ShouldUpdateQuantitySuccessfully()
    {
        AbstractMenuItemOrderAssociation association = new AbstractMenuItemOrderAssociation(_menuItem1, _order1, 5);
       
        association.Update(10);

        Assert.That(association.Quantity, Is.EqualTo(10));
    }

    [Test]
    public void UpdateQuantity_WhenNewQuantityIsNegative_ShouldThrowException()
    {
        AbstractMenuItemOrderAssociation association = new AbstractMenuItemOrderAssociation(_menuItem1, _order1, 5);
        Assert.Throws<ArgumentException>(() => association.Update(-1));
    }
}
