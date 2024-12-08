using CashInn.Enum;
using CashInn.Model;
using CashInn.Model.Employee;

namespace Tests.model;

[TestFixture]
[TestOf(typeof(Kitchen))]
public class KitchenTest
{
    private Branch _branch = null!;
    private Cook _cook = null!;
    private Kitchen _kitchen = null!;
    private const string TestFilePath = "Kitchens.json";

    [SetUp]
    public void SetUp()
    {
        _branch = new Branch(1, "ul. Hermana", "+4857575757");
        _cook = new Cook(2, "John Mikenson", 20000, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8), 
            StatusEmpl.FullTime, true, "Japanese", 2, "Stove", _branch);
        _kitchen = new Kitchen(1, new List<string> { "Oven" }, new List<Cook> { _cook });
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

        Kitchen.ClearExtent();
        Kitchen.LoadExtent();
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _kitchen.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _kitchen.Id = 2;
        Assert.That(_kitchen.Id, Is.EqualTo(2));
    }

    [Test]
    public void Equipment_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _kitchen.Equipment = null);
    }

    [Test]
    public void Equipment_SetEmptyList_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _kitchen.Equipment = new List<string>());
    }

    [Test]
    public void Equipment_SetListWithWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _kitchen.Equipment = new List<string> { "Oven", " " });
    }

    [Test]
    public void Equipment_SetValidList_ShouldSet()
    {
        var equipment = new List<string> { "Oven", "Stove" };
        _kitchen.Equipment = equipment;
        Assert.That(_kitchen.Equipment, Is.EqualTo(equipment));
    }

    [Test]
    public void AddEquipment_WithValidValue_ShouldAdd()
    {
        _kitchen.AddEquipment("Mixer");
        Assert.That(_kitchen.Equipment, Does.Contain("Mixer"));
    }

    [Test]
    public void AddEquipment_WithNullOrWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _kitchen.AddEquipment(null));
        Assert.Throws<ArgumentException>(() => _kitchen.AddEquipment("  "));
    }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredKitchensCorrectly()
    {
        var kitchen1 = new Kitchen(1, new List<string> { "Oven" }, new List<Cook> {_cook});
        var kitchen2 = new Kitchen(2, new List<string> { "Stove" }, new List<Cook> {_cook});

        Kitchen.SaveExtent(); // Assuming you save the extents after creating new kitchens

        Kitchen.LoadExtent();
        Assert.AreEqual(2, Kitchen.GetAll().Count);

        kitchen1 = null!;
        kitchen2 = null!;
        Kitchen.LoadExtent();

        Assert.AreEqual(2, Kitchen.GetAll().Count);
        var loadedKitchens = Kitchen.GetAll();
        Assert.IsTrue(loadedKitchens.Any(k => k.Id == 1));
        Assert.IsTrue(loadedKitchens.Any(k => k.Id == 2));
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