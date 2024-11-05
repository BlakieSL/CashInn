using CashInn.Model;

namespace Tests.model;

[TestFixture]
[TestOf(typeof(Customer))]
public class CustomerTest
{
private Customer _customer = null!;
    private const string TestFilePath = "Customers.json";

    [SetUp]
    public void SetUp()
    {
        _customer = new Customer(1, "John Doe", "123-456-7890", "123 Elm Street", "john@example.com");
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

        Customer.ClearExtent();
        Customer.LoadExtent();
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _customer.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _customer.Id = 2;
        Assert.That(_customer.Id, Is.EqualTo(2));
    }

    [Test]
    public void Name_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _customer.Name = null);
    }

    [Test]
    public void Name_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _customer.Name = "  ");
    }

    [Test]
    public void Name_SetValidValue_ShouldSet()
    {
        _customer.Name = "Jane Smith";
        Assert.That(_customer.Name, Is.EqualTo("Jane Smith"));
    }

    [Test]
    public void Encapsulation_ShouldNotAllowDirectModification()
    {
        var customer = new Customer(1, "John Doe", "123-456-7890", "123 Elm Street", "john@example.com");
        customer.Name = "New Name";

        Assert.AreEqual("New Name", customer.Name);
        Assert.IsTrue(Customer.GetAll().Any(c => c.Name == "New Name"));
    }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredCustomersCorrectly()
    {
        Customer.ClearExtent();
        var customer1 = new Customer(1, "John Doe", "123-456-7890", "123 Elm Street", "john@example.com");
        var customer2 = new Customer(2, "Jane Smith", "098-765-4321", "456 Oak Street", "jane@example.com");

        Customer.SaveExtent();
        Customer.LoadExtent();

        Assert.AreEqual(2, Customer.GetAll().Count);

        customer1 = null!;
        customer2 = null!;
        Customer.LoadExtent();

        Assert.AreEqual(2, Customer.GetAll().Count);
        Assert.IsTrue(Customer.GetAll().Any(c => c.Id == 1));
        Assert.IsTrue(Customer.GetAll().Any(c => c.Id == 2));
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