using CashInn.Model;

namespace Tests.model;

[TestFixture]
[TestOf(typeof(Table))]
public class TableTest
{
    private Table _table = null!;
    private const string TestFilePath = "Tables.json";

    [SetUp]
    public void SetUp()
    {
        _table = new Table(1, 4);
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

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
    public void Capacity_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _table.Capacity = -1);
    }

    [Test]
    public void Capacity_SetZero_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _table.Capacity = 0);
    }

    [Test]
    public void Capacity_SetPositiveValue_ShouldSet()
    {
        _table.Capacity = 6;
        Assert.That(_table.Capacity, Is.EqualTo(6));
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

        table1 = null!;
        table2 = null!;
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