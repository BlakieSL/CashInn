using CashInn.Model;
using NUnit.Framework;

namespace Tests.Association;

[TestFixture]
[TestOf(typeof(Customer))]
public class CustomerReservationAssociationTest
{
    private Customer _customer = null!;
    private Reservation _reservation1 = null!;
    private Reservation _reservation2 = null!;

    [SetUp]
    public void SetUp()
    {
        _customer = new Customer(1, "John Doe", "123-456-7890", "123 Elm St", "john.doe@email.com");
        _reservation1 = new Reservation(101, 2, _customer);
        _reservation2 = new Reservation(102, 4, _customer);
    }

    [Test]
    public void AddReservation_ShouldAddReservationToCustomer()
    {
        _customer.AddReservation(_reservation1);

        Assert.Multiple(() =>
        {
            Assert.That(_customer.Reservations, Contains.Item(_reservation1));
            Assert.That(_reservation1.Customer, Is.EqualTo(_customer));
        });
    }

    [Test]
    public void RemoveReservation_ShouldRemoveReservationFromCustomer()
    {
        _customer.AddReservation(_reservation1);
        _customer.RemoveReservation(_reservation1);

        Assert.Multiple(() =>
        {
            Assert.That(_customer.Reservations, Does.Not.Contain(_reservation1));
        });
    }

    [Test]
    public void AddReservation_ShouldThrowException_WhenReservationAlreadyAssignedToAnotherCustomer()
    {
        var anotherCustomer = new Customer(2, "Jane Doe", "987-654-3210", "456 Oak St", "jane.doe@email.com");

        Assert.Throws<InvalidOperationException>(() => anotherCustomer.AddReservation(_reservation1));
    }

    [Test]
    public void RemoveReservation_ShouldThrowException_WhenReservationNotAssignedToCustomer()
    {
        var anotherCustomer = new Customer(2, "Jane Doe", "987-654-3210", "456 Oak St", "jane.doe@email.com");

        Assert.Throws<InvalidOperationException>(() => anotherCustomer.RemoveReservation(_reservation1));
    }

    [Test]
    public void SetCustomer_ShouldAssignCustomerToReservationSuccessfully()
    {
        var newReservation = new Reservation(103, 3, _customer);

        Assert.Multiple(() =>
        {
            Assert.That(newReservation.Customer, Is.EqualTo(_customer));
            Assert.That(_customer.Reservations, Contains.Item(newReservation));
        });
    }
}
