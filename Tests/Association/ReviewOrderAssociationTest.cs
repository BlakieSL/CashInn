using CashInn.Model;
using CashInn.Enum;
using CashInn.Model.Payment;

namespace Tests.Association
{
    [TestFixture]
    [TestOf(typeof(Review))]
    [TestOf(typeof(Order))]
    public class ReviewOrderAssociationTests
    {
        private Review _review = null!;
        private Order _order = null!;
        private Customer _customer = null!;
        private Payment _payment = null!;

        [SetUp]
        public void SetUp()
        {
            _order = new Order(1, DateTime.Now, true, _customer, _payment);
            _review = new Review(1, Rating.Five, "Excellent service!");
        }

        [Test]
        public void SetReview_ShouldAssignReviewToOrderAndUpdateReverseConnection()
        {
            _order.SetReview(_review);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(_review.Order, _order);
                Assert.AreEqual(_review, _order.Review);
            });
        }

        [Test]
        public void SetReview_WhenReviewAlreadyAssigned_ShouldThrowInvalidOperationException()
        {
            _order.SetReview(_review);

            var newReview = new Review(2, Rating.Four, "Good service.");
            
            Assert.Throws<InvalidOperationException>(() => _order.SetReview(newReview));
        }

        [Test]
        public void SetReview_WhenOrderAlreadyHasReview_ShouldThrowInvalidOperationException()
        {
            var anotherOrder = new Order(2, DateTime.Now, true, _customer, _payment);
            anotherOrder.SetReview(_review);
            
            var anotherReview = new Review(2, Rating.Four, "Nice experience.");
            Assert.Throws<InvalidOperationException>(() => anotherOrder.SetReview(anotherReview));
        }

        [Test]
        public void SetReview_WhenReviewIsAlreadyAssignedToAnotherOrder_ShouldThrowInvalidOperationException()
        {
            var anotherOrder = new Order(2, DateTime.Now, true, _customer, _payment);
            var anotherReview = new Review(2, Rating.Four, "Good service.");
            
            anotherOrder.SetReview(anotherReview);

            Assert.Throws<InvalidOperationException>(() => _order.SetReview(anotherReview));
        }

        [Test]
        public void AddReview_ShouldAssignReviewToOrderCorrectly()
        {
            _order.SetReview(_review);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(_order.Review, _review);
                Assert.AreEqual(_review.Order, _order);
            });
        }
    }
}