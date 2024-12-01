using CashInn.Enum;
using CashInn.Model.Employee;

namespace Tests.model.Employee;

[TestFixture]
[TestOf(typeof(Chef))]
public class ChefCookAssociationTests
{
    private Chef _chef = null!;
    private Cook _cook1 = null!;

    [SetUp]
    public void SetUp()
    {
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
            2
        );

        _cook1 = new Cook(
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
            "Main Kitchen"
        );
    }

    [Test]
    public void AddCook_ShouldAddCookToManagedCooks()
    {
        _chef.AddCook(_cook1);

        Assert.Multiple(() =>
        {
            Assert.That(_chef.ManagedCooks.ToList(), Has.Count.EqualTo(1));
            Assert.That(_chef.ManagedCooks, Contains.Item(_cook1));
            Assert.That(_cook1.Manager, Is.EqualTo(_chef));
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
            1
        );

        anotherChef.AddCook(_cook1);

        Assert.Throws<InvalidOperationException>(() => _chef.AddCook(_cook1));
    }

    [Test]
    public void RemoveCook_ShouldRemoveCookFromManagedCooksAndClearManager()
    {
        _chef.AddCook(_cook1);
        _chef.RemoveCook(_cook1);

        Assert.Multiple(() =>
        {
            Assert.That(_chef.ManagedCooks, Does.Not.Contain(_cook1));
            Assert.That(_cook1.Manager, Is.Null);
        });
    }

    [Test]
    public void RemoveCook_WhenCookNotManaged_ShouldNotThrowException()
    {
        Assert.DoesNotThrow(() => _chef.RemoveCook(_cook1));
    }

    [Test]
    public void SetManager_ShouldUpdateManagerAndAddToManagedCooks()
    {
        _cook1.SetManager(_chef);

        Assert.Multiple(() =>
        {
            Assert.That(_cook1.Manager, Is.EqualTo(_chef));
            Assert.That(_chef.ManagedCooks, Contains.Item(_cook1));
        });
    }

    [Test]
    public void SetManager_ShouldRemoveCookFromPreviousManager()
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
            1
        );

        anotherChef.AddCook(_cook1);
        _cook1.SetManager(_chef);

        Assert.Multiple(() =>
        {
            Assert.That(_cook1.Manager, Is.EqualTo(_chef));
            Assert.That(_chef.ManagedCooks, Contains.Item(_cook1));
            Assert.That(anotherChef.ManagedCooks, Does.Not.Contain(_cook1));
        });
    }

    [Test]
    public void SetManager_ToNull_ShouldRemoveCookFromChef()
    {
        _chef.AddCook(_cook1);
        _cook1.SetManager(null);

        Assert.That(_cook1.Manager, Is.Null);
        Assert.That(_chef.ManagedCooks, Does.Not.Contain(_cook1));
    }

    [Test]
    public void AddCook_ShouldNotCauseInfiniteRecursion()
    {
        Assert.DoesNotThrow(() => _chef.AddCook(_cook1));
    }

    [Test]
    public void RemoveCook_ShouldNotCauseInfiniteRecursion()
    {
        _chef.AddCook(_cook1);
        Assert.DoesNotThrow(() => _chef.RemoveCook(_cook1));
    }

    [Test]
    public void SetManager_ShouldNotCauseInfiniteRecursion()
    {
        Assert.DoesNotThrow(() => _cook1.SetManager(_chef));
    }

    [Test]
    public void ManagedCooks_ShouldBeReadOnly()
    {
        Assert.Throws<NotSupportedException>(() =>
        {
            var cooks = (ICollection<Cook>)_chef.ManagedCooks;
            cooks.Add(_cook1);
        });
    }
}
