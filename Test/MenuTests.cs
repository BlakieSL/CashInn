using NUnit.Framework;
using System;
using System.Collections.Generic;
using CashInn.Model;

namespace CashInn.Test
{
    public class MenuTests
    {
        private Menu _menu;
        private MenuItem _menuItem;

        [SetUp]
        public void Setup()
        {
            _menu = new Menu
            {
                Id = 1,
                DateUpdated = DateTime.Now.AddDays(-1),
                Categories = new List<string> { "Drinks", "Food" },
                MenuItems = new Dictionary<string, ICollection<MenuItem>>()
            };

            _menuItem = new MenuItem
            {
                Id = 1,
                Name = "Burger",
                Price = 10.99,
                Description = "Beef burger",
                DietaryInformation = "Contains gluten",
                Available = true
            };

            _menu.MenuItems.Add("Food", new List<MenuItem> { _menuItem });
        }

        [Test]
        public void Constructor_ShouldInitializeCorrectly()
        {
            Assert.That(_menu.Id.Equals(1));
            Assert.That(_menu.DateUpdated.Date.Equals(DateTime.Now.AddDays(-1).Date));
            Assert.That(_menu.Categories.Count.Equals(2));
            Assert.That(_menu.Categories.Contains("Drinks"));
            Assert.That(_menu.Categories.Contains("Food"));
            Assert.That(_menu.MenuItems["Food"].Count.Equals(1));
        }

        [Test]
        public void SetDateUpdated_ShouldThrowArgumentException_WhenDateIsInTheFuture()
        {
            var ex = Assert.Throws<ArgumentException>(() => _menu.DateUpdated = DateTime.Now.AddDays(1));
            Assert.That(ex.Message.Equals("DateUpdated cannot be in the future (Parameter 'DateUpdated')"));
        }

        [Test]
        public void SaveExtent_ShouldPersistMenus()
        {
            var filePath = "menus.json";
            Menu.SaveMenu(_menu);
            Menu.SaveExtent(filePath);
            Assert.That(System.IO.File.Exists(filePath));
        }


        [Test]
        public void MenuItems_ShouldBeSetCorrectly()
        {
            Assert.That(_menu.MenuItems["Food"].Count.Equals(1));
            Assert.That(_menu.MenuItems["Food"].Contains(_menuItem));
        }

        [Test]
        public void Categories_ShouldBeSetCorrectly()
        {
            Assert.That(_menu.Categories.Count.Equals(2));
            Assert.That(_menu.Categories.Contains("Drinks"));
            Assert.That(_menu.Categories.Contains("Food"));
        }
    }
}
