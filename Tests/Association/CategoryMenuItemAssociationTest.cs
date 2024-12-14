using CashInn.Model;
using CashInn.Model.MenuItem;
using CashInn.Enum;
using NUnit.Framework;

namespace Tests.Association;

[TestFixture]
[TestOf(typeof(Category))]
public class CategoryMenuItemAssociationTests
{
    private Category _category = null!;
    private DefaultItem _menuItem = null!;

    [SetUp]
    public void SetUp()
    {
        _category = new Category(1, "Main Dishes");

        _menuItem = new DefaultItem(
            1,
            "Grilled Chicken",
            12.99,
            "Delicious grilled chicken with herbs",
            "High Protein",
            true,
            ServingSize.Medium,
            _category
        );
    }

    [Test]
    public void AddMenuItem_ShouldAddMenuItemToCategory()
    {
        _category.AddMenuItem(_menuItem);

        Assert.Multiple(() =>
        {
            Assert.That(_category.MenuItems, Contains.Item(_menuItem));
            Assert.That(_menuItem.Category, Is.EqualTo(_category));
        });
    }

    [Test]
    public void AddMenuItem_WhenMenuItemAlreadyAssignedToAnotherCategory_ShouldThrowException()
    {
        var anotherCategory = new Category(2, "Special Dishes");

        Assert.Throws<ArgumentException>(() => anotherCategory.AddMenuItem(_menuItem));
    }

    [Test]
    public void AddMenuItem_ShouldNotCauseInfiniteRecursion()
    {
        Assert.DoesNotThrow(() => _category.AddMenuItem(_menuItem));
    }

    [Test]
    public void RemoveMenuItem_ShouldRemoveMenuItemFromCategoryAndClearCategoryReference()
    {
        _category.AddMenuItem(_menuItem);
        _category.RemoveMenuItem(_menuItem);

        Assert.Multiple(() =>
        {
            Assert.That(_category.MenuItems, Does.Not.Contain(_menuItem));
            Assert.That(_menuItem.Category, Is.Null);
        });
    }

    [Test]
    public void RemoveMenuItem_WhenMenuItemNotInCategory_ShouldNotThrowException()
    {
        Assert.DoesNotThrow(() => _category.RemoveMenuItem(_menuItem));
    }

    [Test]
    public void UpdateMenuItem_ShouldReplaceOldMenuItemWithNewMenuItem()
    {
        var newMenuItem = new DefaultItem(
            2,
            "Steak",
            19.99,
            "Grilled steak with sides",
            "High Protein",
            true,
            ServingSize.Small,
            _category
        );

        _category.AddMenuItem(_menuItem);
        _category.UpdateMenuItem(_menuItem, newMenuItem);

        Assert.Multiple(() =>
        {
            Assert.That(_category.MenuItems, Does.Not.Contain(_menuItem));
            Assert.That(_category.MenuItems, Contains.Item(newMenuItem));
            Assert.That(newMenuItem.Category, Is.EqualTo(_category));
            Assert.That(_menuItem.Category, Is.Null);
        });
    }

    [Test]
    public void UpdateMenuItem_WhenOldMenuItemNotInCategory_ShouldThrowException()
    {
        var newMenuItem = new DefaultItem(
            2,
            "Steak",
            19.99,
            "Grilled steak with sides",
            "High Protein",
            true,
            ServingSize.Small,
            _category
        );

        Assert.DoesNotThrow(() => _category.UpdateMenuItem(_menuItem, newMenuItem));
    }

    [Test]
    public void AddCategory_ShouldSetCategoryForMenuItem()
    {
        _menuItem.AddCategory(_category);

        Assert.Multiple(() =>
        {
            Assert.That(_menuItem.Category, Is.EqualTo(_category));
            Assert.That(_category.MenuItems, Contains.Item(_menuItem));
        });
    }

    [Test]
    public void AddCategory_WhenMenuItemAlreadyInAnotherCategory_ShouldThrowException()
    {
        var anotherCategory = new Category(2, "Special Dishes");
        Assert.Throws<InvalidOperationException>(() => _menuItem.AddCategory(anotherCategory));
    }

    [Test]
    public void RemoveCategory_ShouldClearCategoryForMenuItem()
    {
        _menuItem.AddCategory(_category);
        _menuItem.RemoveCategory();

        Assert.Multiple(() =>
        {
            Assert.That(_menuItem.Category, Is.Null);
            Assert.That(_category.MenuItems, Does.Not.Contain(_menuItem));
        });
    }

    [Test]
    public void RemoveCategory_WhenMenuItemHasNoCategory_ShouldNotThrowException()
    {
        Assert.DoesNotThrow(() => _menuItem.RemoveCategory());
    }

    [Test]
    public void UpdateCategory_ShouldReplaceOldCategoryWithNewCategory()
    {
        var newCategory = new Category(2, "Desserts");

        _menuItem.AddCategory(_category);
        _menuItem.UpdateCategory(newCategory);

        Assert.Multiple(() =>
        {
            Assert.That(_menuItem.Category, Is.EqualTo(newCategory));
            Assert.That(newCategory.MenuItems, Contains.Item(_menuItem));
            Assert.That(_category.MenuItems, Does.Not.Contain(_menuItem));
        });
    }
}
