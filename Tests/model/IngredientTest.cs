using CashInn.Model;

namespace Tests.model;

[TestFixture]
[TestOf(typeof(Ingredient))]
public class IngredientTest
{
    private Ingredient _ingredient = null!;
    private const string TestFilePath = "Ingredients.json";

    [SetUp]
    public void SetUp()
    {
        _ingredient = new Ingredient(1, "Tomato", 20, true);
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

        Ingredient.ClearExtent();
        Ingredient.LoadExtent();
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _ingredient.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _ingredient.Id = 2;
        Assert.That(_ingredient.Id, Is.EqualTo(2));
    }

    [Test]
    public void Name_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _ingredient.Name = null);
    }

    [Test]
    public void Name_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _ingredient.Name = "  ");
    }

    [Test]
    public void Name_SetNonWhiteSpace_ShouldSet()
    {
        _ingredient.Name = "Valid Ingredient";
        Assert.That(_ingredient.Name, Is.EqualTo("Valid Ingredient"));
    }

    [Test]
    public void Calories_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _ingredient.Calories = -10);
    }

    [Test]
    public void Calories_SetPositiveValue_ShouldSet()
    {
        _ingredient.Calories = 50;
        Assert.That(_ingredient.Calories, Is.EqualTo(50));
    }

    [Test]
    public void Encapsulation_ShouldNotAllowDirectModification()
    {
        var ingredient = new Ingredient(1, "Tomato", 20, true);
        ingredient.Name = "New Ingredient";

        Assert.AreEqual("New Ingredient", ingredient.Name);
        Assert.IsTrue(Ingredient.GetAll().Any(i => i.Name == "New Ingredient"));
    }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredIngredientsCorrectly()
    {
        Ingredient.ClearExtent();
        var ingredient1 = new Ingredient(1, "Tomato", 20, true);
        var ingredient2 = new Ingredient(2, "Lettuce", 15, true);

        Ingredient.SaveExtent();
        Ingredient.LoadExtent();

        Assert.AreEqual(2, Ingredient.GetAll().Count);

        ingredient1 = null!;
        ingredient2 = null!;
        Ingredient.LoadExtent();

        Assert.AreEqual(2, Ingredient.GetAll().Count);
        Assert.IsTrue(Ingredient.GetAll().Any(i => i.Id == 1));
        Assert.IsTrue(Ingredient.GetAll().Any(i => i.Id == 2));
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