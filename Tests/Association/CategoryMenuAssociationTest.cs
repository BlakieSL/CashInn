using CashInn.Model;
using NUnit.Framework;
using System;

namespace Tests.Association
{
    [TestFixture]
    [TestOf(typeof(Category))]
    [TestOf(typeof(Menu))]
    public class CategoryMenuAssociationTests
    {
        private Category _category = null!;
        private Menu _menu = null!;

        [SetUp]
        public void SetUp()
        {
            Branch _branch = new Branch(1, "loc", "contact");
            _category = new Category(1, "Main Dishes");
            _menu = new Menu(1, DateTime.Now, _branch);
            
            _menu.AddCategory(new Category(121, "randomName"));
            _category.AddMenu(new Menu(121, DateTime.Now, _branch));
        }

        [Test]
        public void AddMenu_ShouldAddMenuToCategoryAndUpdateReverseConnection()
        {
            _category.AddMenu(_menu);

            Assert.Multiple(() =>
            {
                Assert.That(_category.Menus, Contains.Item(_menu));
                Assert.That(_menu.Categories, Contains.Item(_category));
            });
        }

        [Test]
        public void AddMenu_WhenMenuAlreadyInCategory_ShouldNotDuplicate()
        {
            _category.AddMenu(_menu);
            _category.AddMenu(_menu);
            _category.AddMenu(_menu);
            _category.AddMenu(_menu);

            Assert.That(_category.Menus.Count(), Is.EqualTo(2));
        }

        [Test]
        public void RemoveMenu_ShouldRemoveMenuFromCategoryAndUpdateReverseConnection()
        {
            _category.AddMenu(_menu);
            _category.RemoveMenu(_menu);

            Assert.Multiple(() =>
            {
                Assert.That(_category.Menus, Does.Not.Contain(_menu));
                Assert.That(_menu.Categories, Does.Not.Contain(_category));
            });
        }

        [Test]
        public void RemoveMenu_WhenMenuNotInCategory_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => _category.RemoveMenu(_menu));
        }

        [Test]
        public void AddCategory_ShouldAddCategoryToMenuAndUpdateReverseConnection()
        {
            _menu.AddCategory(_category);

            Assert.Multiple(() =>
            {
                Assert.That(_menu.Categories, Contains.Item(_category));
                Assert.That(_category.Menus, Contains.Item(_menu));
            });
        }

        [Test]
        public void AddCategory_WhenCategoryAlreadyInMenu_ShouldNotDuplicate()
        {
            _menu.AddCategory(_category);
            _menu.AddCategory(_category);
            _menu.AddCategory(_category);
            _menu.AddCategory(_category);

            Assert.That(_menu.Categories.Count(), Is.EqualTo(2));
        }

        [Test]
        public void RemoveCategory_ShouldRemoveCategoryFromMenuAndUpdateReverseConnection()
        {
            _menu.AddCategory(_category);
            _menu.RemoveCategory(_category);

            Assert.Multiple(() =>
            {
                Assert.That(_menu.Categories, Does.Not.Contain(_category));
                Assert.That(_category.Menus, Does.Not.Contain(_menu));
            });
        }

        [Test]
        public void RemoveCategory_WhenCategoryNotInMenu_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => _menu.RemoveCategory(_category));
        }

        [Test]
        public void AddMenuAndAddCategory_ShouldSupportManyToManyRelationship()
        {
            var anotherCategory = new Category(2, "Desserts");

            _category.AddMenu(_menu);
            anotherCategory.AddMenu(_menu);

            Assert.Multiple(() =>
            {
                Assert.That(_category.Menus, Contains.Item(_menu));
                Assert.That(anotherCategory.Menus, Contains.Item(_menu));
                Assert.That(_menu.Categories, Contains.Item(_category));
                Assert.That(_menu.Categories, Contains.Item(anotherCategory));
            });
        }

        [Test]
        public void RemoveMenu_ShouldOnlyRemoveFromCurrentCategory()
        {
            var anotherCategory = new Category(2, "Desserts");
            _category.AddMenu(_menu);
            anotherCategory.AddMenu(_menu);

            _category.RemoveMenu(_menu);

            Assert.Multiple(() =>
            {
                Assert.That(_category.Menus, Does.Not.Contain(_menu));
                Assert.That(anotherCategory.Menus, Contains.Item(_menu));
                Assert.That(_menu.Categories, Contains.Item(anotherCategory));
                Assert.That(_menu.Categories, Does.Not.Contain(_category));
            });
        }

        [Test]
        public void AddMenu_ShouldNotCauseInfiniteRecursion()
        {
            Assert.DoesNotThrow(() => _category.AddMenu(_menu));
        }

        [Test]
        public void RemoveMenu_ShouldNotCauseInfiniteRecursion()
        {
            _category.AddMenu(_menu);
            Assert.DoesNotThrow(() => _category.RemoveMenu(_menu));
        }
    }
}
