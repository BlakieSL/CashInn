using CashInn.Model;
using CashInn.Model.Employee;
using CashInn.Enum;

namespace Tests.Model;

[TestFixture]
[TestOf(typeof(Order))]
public class OrderTest
{
    private Order _order = null!;
    private DeliveryEmpl _deliveryEmployee = null!;
    private Branch _branch = null!;
    private const string TestFilePath = "Orders.json";

    [SetUp]
    public void SetUp()
    {
        // Initialize a Branch
        _branch = new Branch(1, "Downtown Branch", "123-456-7890");

        // Initialize a Delivery Employee
        _deliveryEmployee = new DeliveryEmpl(
            id: 1,
            name: "Alice Johnson",
            salary: 2500.0,
            hireDate: DateTime.Now.AddYears(-1),
            shiftStart: DateTime.Now,
            shiftEnd: DateTime.Now.AddHours(8),
            status: StatusEmpl.FullTime,
            isBranchManager: false,
            vehicle: "Scooter",
            deliveryArea: "Downtown",
            employerBranch: _branch
        );

        // Initialize an Order
        _order = new Order(1, DateTime.Now.AddHours(-1), false);

        // Clean up the test file if it exists
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

        // Clear and load extent
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
    public void AddDeliveryEmployee_NullEmployee_ShouldThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => _order.AddDeliveryEmployee(null!));
    }

    [Test]
    public void AddDeliveryEmployee_ValidEmployee_ShouldAssignEmployee()
    {
        _order.AddDeliveryEmployee(_deliveryEmployee);
        Assert.That(_order.DeliveryEmpl, Is.EqualTo(_deliveryEmployee));
        Assert.That(_deliveryEmployee.AssignedOrders, Contains.Item(_order));
    }

    [Test]
    public void AddDeliveryEmployee_OrderAlreadyAssigned_ShouldThrowException()
    {
        var anotherEmployee = new DeliveryEmpl(
            id: 2,
            name: "Bob Smith",
            salary: 2700.0,
            hireDate: DateTime.Now.AddYears(-2),
            shiftStart: DateTime.Now,
            shiftEnd: DateTime.Now.AddHours(8),
            status: StatusEmpl.FullTime,
            isBranchManager: false,
            vehicle: "Bike",
            deliveryArea: "Uptown",
            employerBranch: _branch
        );

        _order.AddDeliveryEmployee(_deliveryEmployee);

        Assert.Throws<InvalidOperationException>(() => _order.AddDeliveryEmployee(anotherEmployee));
    }

    [Test]
    public void RemoveDeliveryEmployee_NoEmployeeAssigned_ShouldNotThrow()
    {
        Assert.DoesNotThrow(() => _order.RemoveDeliveryEmployee());
        Assert.That(_order.DeliveryEmpl, Is.Null);
    }

    [Test]
    public void RemoveDeliveryEmployee_EmployeeAssigned_ShouldRemoveEmployee()
    {
        _order.AddDeliveryEmployee(_deliveryEmployee);
        _order.RemoveDeliveryEmployee();
        Assert.That(_order.DeliveryEmpl, Is.Null);
        Assert.That(_deliveryEmployee.AssignedOrders, Does.Not.Contain(_order));
    }

    [Test]
    public void UpdateDeliveryEmployee_NullNewEmployee_ShouldThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => _order.UpdateDeliveryEmployee(null!));
    }

    [Test]
    public void UpdateDeliveryEmployee_ValidNewEmployee_ShouldUpdateEmployee()
    {
        var newEmployee = new DeliveryEmpl(
            id: 2,
            name: "Bob Smith",
            salary: 2700.0,
            hireDate: DateTime.Now.AddYears(-2),
            shiftStart: DateTime.Now,
            shiftEnd: DateTime.Now.AddHours(8),
            status: StatusEmpl.FullTime,
            isBranchManager: false,
            vehicle: "Bike",
            deliveryArea: "Uptown",
            employerBranch: _branch
        );

        _order.AddDeliveryEmployee(_deliveryEmployee);
        _order.UpdateDeliveryEmployee(newEmployee);

        Assert.That(_order.DeliveryEmpl, Is.EqualTo(newEmployee));
        Assert.That(newEmployee.AssignedOrders, Contains.Item(_order));
        Assert.That(_deliveryEmployee.AssignedOrders, Does.Not.Contain(_order));
    }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredOrdersCorrectly()
    {
        Order.ClearExtent();
        var order1 = new Order(1, DateTime.Now.AddDays(-1), false);
        var order2 = new Order(2, DateTime.Now.AddDays(-2), true);

        Order.SaveExtent();

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
