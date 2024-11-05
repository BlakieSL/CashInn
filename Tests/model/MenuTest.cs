using CashInn.Model;

namespace Tests.model;

[TestFixture]
[TestOf(typeof(Menu))]
public class MenuTest
{
    private Menu _menu = null!;
    private const string TestFilePath = "Menus.json";

    [SetUp]
    public void SetUp()
    {
        _menu = new Menu(1, DateTime.Now);
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

        Menu.ClearExtent();
        Menu.LoadExtent();
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _menu.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _menu.Id = 2;
        Assert.That(_menu.Id, Is.EqualTo(2));
    }

    [Test]
    public void DateUpdated_SetFutureDate_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _menu.DateUpdated = DateTime.Now.AddDays(1));
    }

    [Test]
    public void DateUpdated_SetPastDate_ShouldSet()
    {
        var pastDate = DateTime.Now.AddDays(-1);
        _menu.DateUpdated = pastDate;
        Assert.That(_menu.DateUpdated, Is.EqualTo(pastDate));
    }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredMenusCorrectly()
    {
        var menu1 = new Menu(1, DateTime.Now.AddDays(-1));
        var menu2 = new Menu(2, DateTime.Now.AddDays(-2));

        Menu.SaveExtent(); // Assuming you save the extents after creating new menus

        Menu.LoadExtent();
        Assert.AreEqual(2, Menu.GetAll().Count);

        menu1 = null!;
        menu2 = null!;
        Menu.LoadExtent();

        Assert.AreEqual(2, Menu.GetAll().Count);
        var loadedMenus = Menu.GetAll();
        Assert.IsTrue(loadedMenus.Any(m => m.Id == 1));
        Assert.IsTrue(loadedMenus.Any(m => m.Id == 2));
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