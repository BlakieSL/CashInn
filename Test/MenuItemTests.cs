using NUnit.Framework;
using System;
using System.Collections.Generic;
using CashInn.Model;

namespace CashInn.Test
{
    public class MenuItemTests
    {
        private MenuItem _menuItem;

        [SetUp]
        public void Setup()
        {
            _menuItem = new MenuItem
            {
                Id = 1,
                Name = "Burger",
                Price = 10.99,
                Description = "A delicious beef burger",
                DietaryInformation = "Contains gluten",
                Available = true
            };
        }

        [Test]
        public void Constructor_ShouldInitializeCorrectly()
        {
            Assert.That(_menuItem.Id.Equals(1));
            Assert.That(_menuItem.Name.Equals("Burger"));
            Assert.That(_menuItem.Price.Equals(10.99));
            Assert.That(_menuItem.Description.Equals("A delicious beef burger"));
            Assert.That(_menuItem.DietaryInformation.Equals("Contains gluten"));
            Assert.That(_menuItem.Available.Equals(true));
        }

        [Test]
        public void SetName_ShouldThrowArgumentException_WhenNameIsNullOrEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => _menuItem.Name = "");
            Assert.That(ex.Message.Equals("Name cannot be null or empty (Parameter 'Name')"));
        }

        [Test]
        public void SetPrice_ShouldThrowArgumentException_WhenPriceIsNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() => _menuItem.Price = -1);
            Assert.That(ex.Message.Equals("Price cannot be negative (Parameter 'Price')"));
        }

        [Test]
        public void SetDescription_ShouldThrowArgumentException_WhenDescriptionIsNullOrEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => _menuItem.Description = "");
            Assert.That(ex.Message.Equals("Description cannot be null or empty (Parameter 'Description')"));
        }

        [Test]
        public void SetDietaryInformation_ShouldThrowArgumentException_WhenDietaryInformationIsNullOrEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => _menuItem.DietaryInformation = "");
            Assert.That(ex.Message.Equals("Dietary information cannot be null or empty (Parameter 'DietaryInformation')"));
        }

        [Test]
        public void SaveMenuItem_ShouldAddMenuItemToCollection()
        {
            var newItem = new MenuItem(2, "Pizza", 12.99, "Delicious cheese pizza", "Vegetarian", true);

            MenuItem.SaveMenuItem(newItem);

            var menuItems = MenuItem.GetAllMenuItems();
            Assert.That(menuItems.Contains(newItem));
        }

        [Test]
        public void SaveExtent_ShouldPersistMenuItems()
        {
            var filePath = "menuItems.json";
            MenuItem.SaveMenuItem(_menuItem);

            MenuItem.SaveExtent(filePath);

            Assert.That(System.IO.File.Exists(filePath));
        }


        [Test]
        public void MenuItem_ShouldInitializeEmptyMenus()
        {
            var newItem = new MenuItem();
            Assert.That(newItem.Menus, Is.Null);
        }

        [Test]
        public void MenuItemProperties_ShouldBeSetCorrectly()
        {
            Assert.That(_menuItem.Name.Equals("Burger"));
            Assert.That(_menuItem.Price.Equals(10.99));
            Assert.That(_menuItem.Description.Equals("A delicious beef burger"));
            Assert.That(_menuItem.DietaryInformation.Equals("Contains gluten"));
            Assert.That(_menuItem.Available.Equals(true));
        }
    }
}
