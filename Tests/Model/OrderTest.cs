using CashInn.Model;

namespace Tests.model;

[TestFixture]
[TestOf(typeof(Order))]
public class OrderTest
{
    private Order _order = null!;
    private const string TestFilePath = "Orders.json";

    [SetUp]
    public void SetUp()
    {
        _order = new Order(1, DateTime.Now, false);
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

        Order.ClearExtent();
        Order.LoadExtent();
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _order.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _order.Id = 2;
        Assert.That(_order.Id, Is.EqualTo(2));
    }

    [Test]
    public void DateAndTime_SetFutureDate_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _order.DateAndTime = DateTime.Now.AddDays(1));
    }

    [Test]
    public void DateAndTime_SetPastDate_ShouldSet()
    {
        var pastDate = DateTime.Now.AddDays(-1);
        _order.DateAndTime = pastDate;
        Assert.That(_order.DateAndTime, Is.EqualTo(pastDate));
    }

    [Test]
    public void IsDelivered_SetTrue_ShouldSet()
    {
        _order.IsDelivered = true;
        Assert.That(_order.IsDelivered, Is.True);
    }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredOrdersCorrectly()
    {
        var order1 = new Order(1, DateTime.Now.AddDays(-1), false);
        var order2 = new Order(2, DateTime.Now.AddDays(-2), true);

        Order.SaveExtent();

        Order.LoadExtent();
        Assert.AreEqual(2, Order.GetAll().Count);

        order1 = null!;
        order2 = null!;
        Order.LoadExtent();

        Assert.AreEqual(2, Order.GetAll().Count);

        var loadedOrders = Order.GetAll();
        Assert.IsTrue(loadedOrders.Any(o => o.Id == 1));
        Assert.IsTrue(loadedOrders.Any(o => o.Id == 2));
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