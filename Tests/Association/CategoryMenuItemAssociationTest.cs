using CashInn.Enum;
using CashInn.Model;
using CashInn.Model.MenuItem;

namespace Tests.Association;

[TestFixture]
[TestOf(typeof(Category))]
public class CategoryMenuItemAssociationTest
{
    private Category _category = null!;
    private DefaultItem _menuItem1 = null!;
    private DefaultItem _menuItem2 = null!;

    [SetUp]
    public void SetUp()
    {
        _category = new Category(1, "Beverages");

        _menuItem1 = new DefaultItem(1, "Coffee", 2.5, "Hot coffee", "None",
            true, ServingSize.Small, _category);

        _menuItem2 = new DefaultItem(2, "Tea", 1.5, "Hot tea", "None",
            true, ServingSize.Small, _category);
    }

    [Test]
    public void AddMenuItem_ShouldAddMenuItemToCategory()
    {
        _category.AddMenuItem(_menuItem1);

        Assert.Multiple(() =>
        {
            Assert.That(_category.MenuItems, Contains.Item(_menuItem1));
            Assert.That(_menuItem1.Category, Is.EqualTo(_category));
        });
    }

    [Test]
    public void RemoveMenuItem_ShouldRemoveMenuItemFromCategory()
    {
        _category.AddMenuItem(_menuItem1);
        _category.RemoveMenuItem(_menuItem1);

        Assert.Multiple(() =>
        {
            Assert.That(_category.MenuItems, Does.Not.Contain(_menuItem1));
        });
    }

    [Test]
    public void AddMenuItem_ShouldThrowException_WhenMenuItemAlreadyBelongsToAnotherCategory()
    {
        var anotherCategory = new Category(2, "Snacks");

        Assert.Throws<ArgumentException>(() => anotherCategory.AddMenuItem(_menuItem1));
    }

    [Test]
    public void RemoveMenuItem_ShouldThrowException_WhenMenuItemNotInCategory()
    {
        var anotherCategory = new Category(2, "Snacks");
        Assert.Throws<InvalidOperationException>(() => anotherCategory.RemoveMenuItem(_menuItem1));
    }

    [Test]
    public void SetCategory_ShouldSetCategorySuccessfully()
    {
        var newCategory = new Category(2, "Desserts");
        _menuItem1.SetCategory(newCategory);

        Assert.Multiple(() =>
        {
            Assert.That(_menuItem1.Category, Is.EqualTo(newCategory));
            Assert.That(newCategory.MenuItems, Contains.Item(_menuItem1));
            Assert.That(_category.MenuItems, Does.Not.Contain(_menuItem1));
        });
    }

    [Test]
    public void SetCategory_WhenAlreadyHasCategory_ShouldUpdateCategorySuccessfully()
    {
        var newCategory = new Category(2, "Desserts");
        _menuItem1.SetCategory(newCategory);

        Assert.Multiple(() =>
        {
            Assert.That(_menuItem1.Category, Is.EqualTo(newCategory));
            Assert.That(newCategory.MenuItems, Contains.Item(_menuItem1));
            Assert.That(_category.MenuItems, Does.Not.Contain(_menuItem1));
        });
    }

    [Test]
    public void RemoveCategory_ShouldRemoveCategorySuccessfully()
    {
        _menuItem1.RemoveCategory();

        Assert.Multiple(() =>
        {
            Assert.That(_category.MenuItems, Does.Not.Contain(_menuItem1));
        });
    }
}