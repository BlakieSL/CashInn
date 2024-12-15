using CashInn.Enum;
using CashInn.Model.MenuItem;
using CashInn.Model;

namespace Tests.Association
{
    [TestFixture]
    [TestOf(typeof(DefaultItem))]
    public class DefaultItemRelatedItemsTests
    {
        private DefaultItem _item1 = null!;
        private DefaultItem _item2 = null!;
        private DefaultItem _item3 = null!;
        private Category _category = null!;

        [SetUp]
        public void SetUp()
        {
            _category = new Category(1, "Main Dishes");

            _item1 = new DefaultItem(1, "Burger", 10.99, "Beef Burger", "High Protein", true, ServingSize.Small, _category);
            _item2 = new DefaultItem(2, "Fries", 4.99, "Crispy Fries", "Vegetarian", true, ServingSize.Medium, _category);
            _item3 = new DefaultItem(3, "Salad", 7.99, "Fresh Salad", "Low Calorie", true, ServingSize.Small, _category);
        }

        [Test]
        public void AddRelatedItem_ShouldAddBidirectionalRelationship()
        {
            _item1.AddRelatedItem(_item2);

            Assert.Multiple(() =>
            {
                Assert.That(_item1.RelatedItems, Contains.Item(_item2));
                Assert.That(_item2.ReferencingItems, Contains.Item(_item1));
            });
        }

        [Test]
        public void AddRelatedItem_WhenItemIsSelf_ShouldThrowException()
        {
            Assert.Throws<InvalidOperationException>(() => _item1.AddRelatedItem(_item1));
        }

        [Test]
        public void AddRelatedItem_WhenAlreadyRelated_ShouldNotDuplicate()
        {
            _item1.AddRelatedItem(_item2);
            _item1.AddRelatedItem(_item2);

            Assert.Multiple(() =>
            {
                Assert.That(_item1.RelatedItems.Count(), Is.EqualTo(1));
                Assert.That(_item2.ReferencingItems.Count(), Is.EqualTo(1));
            });
        }

        [Test]
        public void RemoveRelatedItem_ShouldRemoveBidirectionalRelationship()
        {
            _item1.AddRelatedItem(_item2);
            _item1.RemoveRelatedItem(_item2);

            Assert.Multiple(() =>
            {
                Assert.That(_item1.RelatedItems, Does.Not.Contain(_item2));
                Assert.That(_item2.ReferencingItems, Does.Not.Contain(_item1));
            });
        }

        [Test]
        public void RemoveRelatedItem_WhenNotRelated_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => _item1.RemoveRelatedItem(_item2));
        }

        [Test]
        public void AddAndRemoveMultipleRelatedItems_ShouldMaintainConsistency()
        {
            _item1.AddRelatedItem(_item2);
            _item1.AddRelatedItem(_item3);

            Assert.Multiple(() =>
            {
                Assert.That(_item1.RelatedItems, Contains.Item(_item2));
                Assert.That(_item1.RelatedItems, Contains.Item(_item3));
                Assert.That(_item2.ReferencingItems, Contains.Item(_item1));
                Assert.That(_item3.ReferencingItems, Contains.Item(_item1));
            });

            _item1.RemoveRelatedItem(_item2);

            Assert.Multiple(() =>
            {
                Assert.That(_item1.RelatedItems, Does.Not.Contain(_item2));
                Assert.That(_item2.ReferencingItems, Does.Not.Contain(_item1));
                Assert.That(_item1.RelatedItems, Contains.Item(_item3));
                Assert.That(_item3.ReferencingItems, Contains.Item(_item1));
            });
        }

        [Test]
        public void RemoveRelatedItem_ShouldNotAffectOtherRelationships()
        {
            _item1.AddRelatedItem(_item2);
            _item1.AddRelatedItem(_item3);

            _item1.RemoveRelatedItem(_item2);

            Assert.Multiple(() =>
            {
                Assert.That(_item1.RelatedItems, Contains.Item(_item3));
                Assert.That(_item3.ReferencingItems, Contains.Item(_item1));
                Assert.That(_item2.ReferencingItems, Does.Not.Contain(_item1));
            });
        }
    }
}
