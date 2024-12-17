using CashInn.Model;
using CashInn.Model.Payment;

namespace Tests.Association
{
    [TestFixture]
    [TestOf(typeof(Order))]
    public class OrderPaymentAssociationTest
    {
        private Order _order = null!;
        private AbstractPayment _payment = null!;

        [SetUp]
        public void SetUp()
        {
            _order = new Order(1, DateTime.Today, false);
            _payment = new CardPayment(101, 50.0, DateTime.Today, "number");
        }

        [Test]
        public void AddPayment_ShouldAssociatePaymentWithOrder()
        {
            _order.AddPayment(_payment);

            Assert.Multiple(() =>
            {
                Assert.That(_order.Payment, Is.EqualTo(_payment));
                Assert.That(_payment.Order, Is.EqualTo(_order));
            });
        }

        [Test]
        public void AddPayment_ShouldThrowException_WhenPaymentAlreadyAssignedToAnotherOrder()
        {
            _payment.SetOrder(_order);
            var anotherOrder = new Order(2, DateTime.Today, true);

            Assert.Throws<InvalidOperationException>(() => anotherOrder.AddPayment(_payment));
        }
    }

    [TestFixture]
    [TestOf(typeof(AbstractPayment))]
    public class PaymentOrderAssociationTest
    {
        private Order _order = null!;
        private AbstractPayment _payment = null!;

        [SetUp]
        public void SetUp()
        {
            _order = new Order(1, DateTime.Today, false);
            _payment = new CardPayment(101, 50.0, DateTime.Today, "number");
        }

        [Test]
        public void SetOrder_ShouldAssignOrderToPayment()
        {
            var newOrder = new Order(2, DateTime.Today, true);

            _payment.SetOrder(newOrder);

            Assert.Multiple(() =>
            {
                Assert.That(_payment.Order, Is.EqualTo(newOrder));
                Assert.That(newOrder.Payment, Is.EqualTo(_payment));
            });
        }

        [Test]
        public void SetOrder_ShouldThrowException_WhenPaymentIsAlreadyAssignedToAnotherOrder()
        {
            _payment.SetOrder(_order);
            var anotherOrder = new Order(2, DateTime.Today, true);

            Assert.Throws<InvalidOperationException>(() => _payment.SetOrder(anotherOrder));
        }
    }
}