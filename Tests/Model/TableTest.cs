using CashInn.Model;
using CashInn.Model.Employee;

namespace Tests.Model;

[TestFixture]
[TestOf(typeof(Table))]
public class TableTest
{
    private Table _table = null!;
    private Waiter _waiter = null!;
    private Branch _branch = null!;
    private const string TestFilePath = "Tables.json";

    [SetUp]
    public void SetUp()
    {
        // Initialize a branch
        _branch = new Branch(1, "Main Street, Warsaw", "+48 123 456 789");

        // Initialize a waiter assigned to the branch
        _waiter = new Waiter(1, "John Doe", 3000.0, DateTime.Now.AddYears(-1), DateTime.Now, DateTime.Now.AddHours(8),
            CashInn.Enum.StatusEmpl.FullTime, false, 200.0, _branch);

        // Initialize a table
        _table = new Table(1, 4);

        // Clean up the test file if it exists
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

        // Clear and load extent
        Table.ClearExtent();
        Table.LoadExtent();
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _table.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _table.Id = 2;
        Assert.That(_table.Id, Is.EqualTo(2));
    }

    [Test]
    public void Capacity_SetZero_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _table.Capacity = 0);
    }

    [Test]
    public void Capacity_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _table.Capacity = -1);
    }

    [Test]
    public void Capacity_SetPositiveValue_ShouldSet()
    {
        _table.Capacity = 6;
        Assert.That(_table.Capacity, Is.EqualTo(6));
    }

    [Test]
    public void AddWaiter_NullWaiter_ShouldThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => _table.AddWaiter(null!));
    }

    [Test]
    public void AddWaiter_ValidWaiter_ShouldSetWaiter()
    {
        _table.AddWaiter(_waiter);
        Assert.That(_table.Waiter, Is.EqualTo(_waiter));
        Assert.That(_waiter.AssignedTables, Contains.Item(_table));
    }

    [Test]
    public void AddWaiter_TableAlreadyAssignedToAnotherWaiter_ShouldThrowException()
    {
        var anotherWaiter = new Waiter(2, "Jane Smith", 2500.0, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8),
            CashInn.Enum.StatusEmpl.FullTime, false, 150.0, _branch);

        _table.AddWaiter(_waiter);

        Assert.Throws<InvalidOperationException>(() => _table.AddWaiter(anotherWaiter));
    }

    [Test]
    public void RemoveWaiter_NoWaiterAssigned_ShouldNotThrow()
    {
        Assert.DoesNotThrow(() => _table.RemoveWaiter());
        Assert.That(_table.Waiter, Is.Null);
    }

    [Test]
    public void RemoveWaiter_WaiterAssigned_ShouldRemoveWaiter()
    {
        _table.AddWaiter(_waiter);
        _table.RemoveWaiter();
        Assert.That(_table.Waiter, Is.Null);
        Assert.That(_waiter.AssignedTables, Does.Not.Contain(_table));
    }

    [Test]
    public void UpdateWaiter_NullNewWaiter_ShouldThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => _table.UpdateWaiter(null!));
    }

    [Test]
    public void UpdateWaiter_ValidNewWaiter_ShouldUpdateWaiter()
    {
        var newWaiter = new Waiter(2, "Jane Smith", 2500.0, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8),
            CashInn.Enum.StatusEmpl.FullTime, false, 150.0, _branch);

        _table.AddWaiter(_waiter);
        _table.UpdateWaiter(newWaiter);

        Assert.That(_table.Waiter, Is.EqualTo(newWaiter));
        Assert.That(newWaiter.AssignedTables, Contains.Item(_table));
        Assert.That(_waiter.AssignedTables, Does.Not.Contain(_table));
    }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredTablesCorrectly()
    {
        Table.ClearExtent();
        var table1 = new Table(1, 4);
        var table2 = new Table(2, 6);

        Table.SaveExtent();

        Table.LoadExtent();
        Assert.AreEqual(2, Table.GetAll().Count);

        var loadedTables = Table.GetAll();
        Assert.IsTrue(loadedTables.Any(t => t.Id == 1));
        Assert.IsTrue(loadedTables.Any(t => t.Id == 2));
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
