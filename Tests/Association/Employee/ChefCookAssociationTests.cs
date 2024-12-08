using CashInn.Enum;
using CashInn.Model;
using CashInn.Model.Employee;

namespace Tests.model.Employee;

[TestFixture]
[TestOf(typeof(Chef))]
public class ChefCookAssociationTests
{
    private Chef _chef = null!;
    private Cook _cook = null!;
    private Branch _branch = null!;

    [SetUp]
    public void SetUp()
    {
        _branch = new Branch(1, "ul.Hermana", "+485757575");
        _chef = new Chef(
            1,
            "Test Chef",
            50000,
            DateTime.Now.AddYears(-5),
            DateTime.Today.AddHours(10),
            DateTime.Today.AddHours(18),
            StatusEmpl.FullTime,
            false,
            "French",
            10,
            2,
            _branch
        );

        _cook = new Cook(
            2,
            "Test Cook 1",
            30000,
            DateTime.Now.AddYears(-3),
            DateTime.Today.AddHours(9),
            DateTime.Today.AddHours(17),
            StatusEmpl.FullTime,
            false,
            "Italian",
            5,
            "Main Kitchen",
            _branch
        );
    }

    [Test]
    public void AddCook_ShouldAddCookToManagedCooks()
    {
        _chef.AddCook(_cook);

        Assert.Multiple(() =>
        {
            Assert.That(_chef.ManagedCooks.ToList(), Has.Count.EqualTo(1));
            Assert.That(_chef.ManagedCooks, Contains.Item(_cook));
            Assert.That(_cook.Manager, Is.EqualTo(_chef));
        });
    }

    [Test]
    public void AddCook_WhenCookAlreadyManagedByAnotherChef_ShouldThrowException()
    {
        var anotherChef = new Chef(
            4,
            "Another Chef",
            45000,
            DateTime.Now.AddYears(-3),
            DateTime.Today.AddHours(11),
            DateTime.Today.AddHours(19),
            StatusEmpl.PartTime,
            true,
            "Mexican",
            7,
            1,
            _branch
        );

        anotherChef.AddCook(_cook);

        Assert.Throws<InvalidOperationException>(() => _chef.AddCook(_cook));
    }

    [Test]
    public void AddCook_ShouldNotCauseInfiniteRecursion()
    {
        Assert.DoesNotThrow(() => _chef.AddCook(_cook));
    }

    [Test]
    public void RemoveCook_ShouldRemoveCookFromManagedCooksAndClearManager()
    {
        _chef.AddCook(_cook);
        _chef.RemoveCook(_cook);

        Assert.Multiple(() =>
        {
            Assert.That(_chef.ManagedCooks, Does.Not.Contain(_cook));
            Assert.That(_cook.Manager, Is.Null);
        });
    }

    [Test]
    public void RemoveCook_WhenCookNotManaged_ShouldNotThrowException()
    {
        Assert.DoesNotThrow(() => _chef.RemoveCook(_cook));
    }

    [Test]
    public void ManagedCooks_ShouldBeReadOnly()
    {
        Assert.Throws<NotSupportedException>(() =>
        {
            var cooks = (ICollection<Cook>)_chef.ManagedCooks;
            cooks.Add(_cook);
        });
    }

    [Test]
    public void AddManager_ShouldSetManagerForCook()
    {
        _cook.AddManager(_chef);

        Assert.Multiple(() =>
        {
            Assert.That(_cook.Manager, Is.EqualTo(_chef));
            Assert.That(_chef.ManagedCooks, Contains.Item(_cook));
        });
    }

    [Test]
    public void AddManager_WhenCookAlreadyHasManager_ShouldThrowException()
    {
        var anotherChef = new Chef(
            3,
            "Another Chef",
            45000,
            DateTime.Now.AddYears(-3),
            DateTime.Today.AddHours(11),
            DateTime.Today.AddHours(19),
            StatusEmpl.PartTime,
            true,
            "Mexican",
            7,
            1,
            _branch
        );

        _cook.AddManager(anotherChef);

        Assert.Throws<InvalidOperationException>(() => _cook.AddManager(_chef));
    }

    [Test]
    public void RemoveManager_ShouldClearManagerForCook()
    {
        _cook.AddManager(_chef);
        _cook.RemoveManager();

        Assert.Multiple(() =>
        {
            Assert.That(_cook.Manager, Is.Null);
            Assert.That(_chef.ManagedCooks, Does.Not.Contain(_cook));
        });
    }

    [Test]
    public void RemoveManager_WhenCookHasNoManager_ShouldNotThrowException()
    {
        Assert.DoesNotThrow(() => _cook.RemoveManager());
    }

    [Test]
    public void UpdateCook_ShouldReplaceOldCookWithNewCook()
    {
        var newCook = new Cook(
            3,
            "New Cook",
            32000,
            DateTime.Now.AddYears(-2),
            DateTime.Today.AddHours(8),
            DateTime.Today.AddHours(16),
            StatusEmpl.FullTime,
            false,
            "Japanese",
            4,
            "Grill Station",
            _branch
        );

        _chef.AddCook(_cook);
        _chef.UpdateCook(_cook, newCook);

        Assert.Multiple(() =>
        {
            Assert.That(_chef.ManagedCooks, Does.Not.Contain(_cook));
            Assert.That(_chef.ManagedCooks, Contains.Item(newCook));
            Assert.That(newCook.Manager, Is.EqualTo(_chef));
            Assert.That(_cook.Manager, Is.Null);
        });
    }

    [Test]
    public void UpdateCook_WhenOldCookNotManaged_ShouldThrowException()
    {
        var newCook = new Cook(
            3,
            "New Cook",
            32000,
            DateTime.Now.AddYears(-2),
            DateTime.Today.AddHours(8),
            DateTime.Today.AddHours(16),
            StatusEmpl.FullTime,
            false,
            "Japanese",
            4,
            "Grill Station",
            _branch
        );

        Assert.Throws<InvalidOperationException>(() => _chef.UpdateCook(_cook, newCook));
    }

    [Test]
    public void UpdateManager_ShouldReplaceOldManagerWithNewManager()
    {
        var newChef = new Chef(
            3,
            "New Chef",
            55000,
            DateTime.Now.AddYears(-4),
            DateTime.Today.AddHours(12),
            DateTime.Today.AddHours(20),
            StatusEmpl.FullTime,
            false,
            "Italian",
            8,
            3,
            _branch
        );

        _cook.AddManager(_chef);
        _cook.UpdateManager(newChef);

        Assert.Multiple(() =>
        {
            Assert.That(_cook.Manager, Is.EqualTo(newChef));
            Assert.That(newChef.ManagedCooks, Contains.Item(_cook));
            Assert.That(_chef.ManagedCooks, Does.Not.Contain(_cook));
        });
    }

    [Test]
    public void UpdateManager_WhenCookHasNoManager_ShouldSetNewManager()
    {
        var newChef = new Chef(
            3,
            "New Chef",
            55000,
            DateTime.Now.AddYears(-4),
            DateTime.Today.AddHours(12),
            DateTime.Today.AddHours(20),
            StatusEmpl.FullTime,
            false,
            "Italian",
            8,
            3,
            _branch
        );

        _cook.UpdateManager(newChef);

        Assert.Multiple(() =>
        {
            Assert.That(_cook.Manager, Is.EqualTo(newChef));
            Assert.That(newChef.ManagedCooks, Contains.Item(_cook));
        });
    }
}
