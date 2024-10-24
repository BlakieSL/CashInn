using NUnit.Framework;
using CashInn.Model;
using CashInn.Enum;
using System;

namespace CashInn.Test
{
    public class WaiterTests
    {
        private Waiter _waiter;

        [SetUp]
        public void Setup()
        {
            _waiter = new Waiter(
                id: 1,
                name: "Alice Johnson",
                salary: 2500.00,
                hireDate: DateTime.Now.AddYears(-1),
                shiftStart: DateTime.Now.AddHours(-6),
                shiftEnd: DateTime.Now,
                status: StatusEmpl.PartTime,
                isBranchManager: false,
                tipsEarned: 200.00
            );
        }

        [Test]
        public void Constructor_ShouldInitializeCorrectly()
        {
            Assert.That(_waiter.Id.Equals(1));
            Assert.That(_waiter.Name.Equals("Alice Johnson"));
            Assert.That(_waiter.Salary.Equals(2500.00));
            Assert.That(_waiter.TipsEarned.Equals(200.00));
            Assert.That(_waiter.Status.Equals(StatusEmpl.PartTime));
            Assert.That(_waiter.EmployeeType.Equals("Waiter"));
        }

        [Test]
        public void TipsEarned_SetWithNegativeValue_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _waiter.TipsEarned = -100);
            Assert.That(ex.Message.Equals("Tips earned cannot be negative (Parameter 'TipsEarned')"));
        }

        [Test]
        public void Constructor_WithNegativeTipsEarned_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Waiter(
                    id: 1,
                    name: "Alice Johnson",
                    salary: 2500.00,
                    hireDate: DateTime.Now.AddYears(-1),
                    shiftStart: DateTime.Now.AddHours(-6),
                    shiftEnd: DateTime.Now,
                    status: StatusEmpl.PartTime,
                    isBranchManager: false,
                    tipsEarned: -50  // Invalid negative tips
                )
            );
            Assert.That(ex.Message.Equals("Tips earned cannot be negative (Parameter 'TipsEarned')"));
        }
    }
}
