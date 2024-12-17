using CashInn.Model;
using CashInn.Enum;
using CashInn.Model.Payment;
using NUnit.Framework;

namespace Tests.Association;

[TestFixture]
[TestOf(typeof(Customer))]
public class CustomerReviewAssociationTest
{
    private Customer _customer = null!;
    private Order _order = null!;
    private AbstractPayment _payment = null!;
    private Review _review1 = null!;

    [SetUp]
    public void SetUp()
    {
        _customer = new Customer(1, "John Doe", "1234567890",
            "123 Main St", "john.doe@example.com");
        _order = new Order(1, DateTime.Today, true);
        _review1 = new Review(1, Rating.One, "Great service!", _customer, _order);
    }

    [Test]
    public void AddReview_ShouldAddReviewToCustomer()
    {
        _customer.AddReview(_review1);

        Assert.Multiple(() =>
        {
            Assert.That(_customer.Reviews, Contains.Item(_review1));
            Assert.That(_review1.Customer, Is.EqualTo(_customer));
        });
    }

    [Test]
    public void RemoveReview_ShouldRemoveReviewFromCustomer()
    {
        _customer.AddReview(_review1);
        _customer.RemoveReview(_review1);

        Assert.Multiple(() =>
        {
            Assert.That(_customer.Reviews, Does.Not.Contain(_review1));
        });
    }

    [Test]
    public void AddReview_ShouldThrowException_WhenReviewAlreadyBelongsToAnotherCustomer()
    {
        var anotherCustomer = new Customer(2, "Jane Doe", "0987654321",
            "456 Elm St", "jane.doe@example.com");

        Assert.Throws<InvalidOperationException>(() => anotherCustomer.AddReview(_review1));
    }

    [Test]
    public void RemoveReview_ShouldThrowException_WhenReviewNotInCustomer()
    {
        var anotherCustomer = new Customer(2, "Jane Doe", "0987654321",
            "456 Elm St", "jane.doe@example.com");
        Assert.Throws<InvalidOperationException>(() => anotherCustomer.RemoveReview(_review1));
    }

    [Test]
    public void SetCustomer_ShouldSetCustomerSuccessfully()
    {
        var newCustomer = new Customer(2, "Jane Doe", "0987654321",
            "456 Elm St", "jane.doe@example.com");
        var newReview = new Review(1, Rating.One, "Great service!");

        newReview.SetCustomer(newCustomer);

        Assert.Multiple(() =>
        {
            Assert.That(newReview.Customer, Is.EqualTo(newCustomer));
            Assert.That(newCustomer.Reviews, Contains.Item(newReview));
        });
    }
}