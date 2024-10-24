using NUnit.Framework;
using CashInn.Model;
using System;
using System.Collections.Generic;

namespace CashInn.Test
{
    public class BranchTests
    {
        private Branch _branch;

        [SetUp]
        public void Setup()
        {
            _branch = new Branch(1, "Main Street", "123-456-789");
        }

        [Test]
        public void Constructor_ShouldInitializeCorrectly()
        {
            Assert.That(_branch.Id.Equals(1));
            Assert.That(_branch.Location.Equals("Main Street"));
            Assert.That(_branch.ContactInfo.Equals("123-456-789"));
        }

        [Test]
        public void SetLocation_ShouldThrowArgumentException_WhenLocationIsNullOrEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => _branch.Location = "");
            Assert.That(ex.Message.Equals("Location cannot be null or empty (Parameter 'Location')"));
        }

        [Test]
        public void SetContactInfo_ShouldThrowArgumentException_WhenContactInfoIsNullOrEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => _branch.ContactInfo = "");
            Assert.That(ex.Message.Equals("Contact info cannot be null or empty (Parameter 'ContactInfo')"));
        }

        [Test]
        public void SaveExtent_ShouldPersistBranches()
        {
            var filePath = "branches.json";
            Branch.SaveBranch(_branch);
            Branch.SaveExtent(filePath);
            Assert.That(System.IO.File.Exists(filePath));
        }
    }
}
