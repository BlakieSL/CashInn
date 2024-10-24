using NUnit.Framework;
using CashInn.Model;
using System;
using System.Collections.Generic;

namespace CashInn.Test
{
    public class TableTests
    {
        private Table _table;

        [SetUp]
        public void Setup()
        {
            _table = new Table
            {
                Id = 1,
                Capacity = 4
            };
        }

        [Test]
        public void Constructor_ShouldInitializeCorrectly()
        {
            Assert.That(_table.Id.Equals(1));
            Assert.That(_table.Capacity.Equals(4));
        }

        [Test]
        public void SetCapacity_ShouldThrowArgumentException_WhenCapacityIsLessThanOrEqualToZero()
        {
            var ex = Assert.Throws<ArgumentException>(() => _table.Capacity = 0);
            Assert.That(ex.Message.Equals("Capacity must be greater than zero (Parameter 'Capacity')"));
        }

        [Test]
        public void SaveExtent_ShouldPersistTables()
        {
            var filePath = "tables.json";
            Table.SaveTable(_table);
            Table.SaveExtent(filePath);
            Assert.That(System.IO.File.Exists(filePath));
        }
    }
}