using CashInn.Model;
using CashInn.Model.MenuItem;
using NUnit.Framework;
using System;
using CashInn.Enum;

namespace Tests.Association
{
    [TestFixture]
    [TestOf(typeof(Ingredient))]
    [TestOf(typeof(AbstractMenuItem))]
    public class IngredientMenuItemAssociationTests
    {
        private Ingredient _ingredient = null!;
        private AbstractMenuItem _menuItem = null!;
        private Ingredient _anotherIngredient = null!;
        private AbstractMenuItem _anotherMenuItem = null!;

        [SetUp]
        public void SetUp()
        {
            _ingredient = new Ingredient(1, "Tomato", 20, true);
            _menuItem = new DefaultItem(
                1,
                "Salad",
                5.99,
                "Fresh tomato salad",
                "Vegan",
                true,
                ServingSize.Small,
                new Category(1, "Starters")
            );
            _menuItem.AddIngredient(new Ingredient(121, "random", 2, true));
            _anotherIngredient = new Ingredient(2, "Cheese", 80, true);
            _anotherMenuItem = new DefaultItem(
                2,
                "Cheese Sandwich",
                7.99,
                "Sandwich with cheese",
                "Vegetarian",
                true,
                ServingSize.Medium,
                new Category(2, "Main Course")
            );
            _anotherMenuItem.AddIngredient(new Ingredient(122, "random", 2, true));
        }

        [Test]
        public void AddIngredient_ShouldAddIngredientToMenuItemAndUpdateReverseConnection()
        {
            _menuItem.AddIngredient(_ingredient);

            Assert.Multiple(() =>
            {
                CollectionAssert.Contains(_menuItem.Ingredients, _ingredient);
                CollectionAssert.Contains(_ingredient.MenuItems, _menuItem);
            });
        }

        [Test]
        public void AddIngredient_WhenIngredientAlreadyAdded_ShouldNotDuplicate()
        {
            _menuItem.AddIngredient(_ingredient);
            _menuItem.AddIngredient(_ingredient);

            Assert.That(_menuItem.Ingredients, Is.EquivalentTo(new[] { _ingredient }));
        }

        [Test]
        public void RemoveIngredient_ShouldRemoveIngredientFromMenuItemAndUpdateReverseConnection()
        {
            _menuItem.AddIngredient(_ingredient);
            _menuItem.RemoveIngredient(_ingredient);

            Assert.Multiple(() =>
            {
                CollectionAssert.DoesNotContain(_menuItem.Ingredients, _ingredient);
                CollectionAssert.DoesNotContain(_ingredient.MenuItems, _menuItem);
            });
        }

        [Test]
        public void RemoveIngredient_WhenIngredientNotInMenuItem_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => _menuItem.RemoveIngredient(_ingredient));
        }

        [Test]
        public void AddMenuItem_ShouldAddMenuItemToIngredientAndUpdateReverseConnection()
        {
            _ingredient.AddMenuItem(_menuItem);

            Assert.Multiple(() =>
            {
                CollectionAssert.Contains(_ingredient.MenuItems, _menuItem);
                CollectionAssert.Contains(_menuItem.Ingredients, _ingredient);
            });
        }

        [Test]
        public void AddMenuItem_WhenMenuItemAlreadyInIngredient_ShouldNotDuplicate()
        {
            _ingredient.AddMenuItem(_menuItem);
            _ingredient.AddMenuItem(_menuItem);

            Assert.That(_ingredient.MenuItems, Is.EquivalentTo(new[] { _menuItem }));
        }

        [Test]
        public void RemoveMenuItem_ShouldRemoveMenuItemFromIngredientAndUpdateReverseConnection()
        {
            _ingredient.AddMenuItem(_menuItem);
            _ingredient.RemoveMenuItem(_menuItem);

            Assert.Multiple(() =>
            {
                CollectionAssert.DoesNotContain(_ingredient.MenuItems, _menuItem);
                CollectionAssert.DoesNotContain(_menuItem.Ingredients, _ingredient);
            });
        }

        [Test]
        public void RemoveMenuItem_WhenMenuItemNotInIngredient_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => _ingredient.RemoveMenuItem(_menuItem));
        }

        [Test]
        public void AddIngredientAndAddMenuItem_ShouldSupportManyToManyRelationship()
        {
            _menuItem.AddIngredient(_ingredient);
            _menuItem.AddIngredient(_anotherIngredient);

            _anotherMenuItem.AddIngredient(_ingredient);

            Assert.Multiple(() =>
            {
                CollectionAssert.Contains(_menuItem.Ingredients, _ingredient);
                CollectionAssert.Contains(_menuItem.Ingredients, _anotherIngredient);

                CollectionAssert.Contains(_anotherMenuItem.Ingredients, _ingredient);

                CollectionAssert.Contains(_ingredient.MenuItems, _menuItem);
                CollectionAssert.Contains(_ingredient.MenuItems, _anotherMenuItem);
            });
        }

        [Test]
        public void RemoveIngredient_ShouldNotAffectOtherMenuItems()
        {
            _menuItem.AddIngredient(_ingredient);
            _anotherMenuItem.AddIngredient(_ingredient);

            _menuItem.RemoveIngredient(_ingredient);

            Assert.Multiple(() =>
            {
                CollectionAssert.DoesNotContain(_menuItem.Ingredients, _ingredient);
                CollectionAssert.Contains(_anotherMenuItem.Ingredients, _ingredient);

                CollectionAssert.Contains(_ingredient.MenuItems, _anotherMenuItem);
                CollectionAssert.DoesNotContain(_ingredient.MenuItems, _menuItem);
            });
        }

        [Test]
        public void AddIngredient_ShouldNotCauseInfiniteRecursion()
        {
            Assert.DoesNotThrow(() => _menuItem.AddIngredient(_ingredient));
        }

        [Test]
        public void RemoveIngredient_ShouldNotCauseInfiniteRecursion()
        {
            _menuItem.AddIngredient(_ingredient);
            Assert.DoesNotThrow(() => _menuItem.RemoveIngredient(_ingredient));
        }
    }
}
