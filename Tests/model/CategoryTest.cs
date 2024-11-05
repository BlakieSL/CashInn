using CashInn.Model;

namespace Tests.model;

[TestFixture]
[TestOf(typeof(Category))]
public class CategoryTest
{
    private Category _category = null!;
    private const string TestFilePath = "Categories.json";

    [SetUp]
    public void SetUp()
    {
        _category = new Category(1, "Food");
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

        Category.ClearExtent();
        Category.LoadExtent();
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _category.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _category.Id = 2;
        Assert.That(_category.Id, Is.EqualTo(2));
    }

    [Test]
    public void Name_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _category.Name = null);
    }

    [Test]
    public void Name_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _category.Name = "  ");
    }

    [Test]
    public void Name_SetValidValue_ShouldSet()
    {
        _category.Name = "Beverages";
        Assert.That(_category.Name, Is.EqualTo("Beverages"));
    }

    [Test]
    public void Encapsulation_ShouldNotAllowDirectModification()
    {
        var category = new Category(1, "Food");
        category.Name = "New Name";

        Assert.AreEqual("New Name", category.Name);
        Assert.IsTrue(Category.GetAll().Any(c => c.Name == "New Name"));
    }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredCategoriesCorrectly()
    {
        Category.ClearExtent();
        var category1 = new Category(1, "Food");
        var category2 = new Category(2, "Beverages");

        Category.SaveExtent();
        Category.LoadExtent();

        Assert.AreEqual(2, Category.GetAll().Count);

        category1 = null!;
        category2 = null!;
        Category.LoadExtent();

        Assert.AreEqual(2, Category.GetAll().Count);
        Assert.IsTrue(Category.GetAll().Any(c => c.Id == 1));
        Assert.IsTrue(Category.GetAll().Any(c => c.Id == 2));
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }
    }
}