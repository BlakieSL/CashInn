using NUnit.Framework;
using CashInn.Model;
using CashInn.Enum;
using System;

namespace CashInn.Test
{
    public class FlexibleEmplTests
    {
        private FlexibleEmpl _flexibleEmpl;

        [SetUp]
        public void Setup()
        {
            _flexibleEmpl = new FlexibleEmpl(
                id: 1,
                name: "John Doe",
                salary: 3000.00,
                hireDate: DateTime.Now.AddYears(-2),
                shiftStart: DateTime.Now.AddHours(-5),
                shiftEnd: DateTime.Now,
                status: StatusEmpl.FullTime,
                isBranchManager: false,
                vehicle: "Car",
                deliveryArea: "Downtown",
                tipsEarned: 100.00
            );
        }

        [Test]
        public void Constructor_ShouldInitializeCorrectly()
        {
            Assert.That(_flexibleEmpl.Id.Equals(1));
            Assert.That("John Doe".Equals(_flexibleEmpl.Name));
            Assert.That(_flexibleEmpl.Salary.Equals(3000.00));
            Assert.That(_flexibleEmpl.Vehicle.Equals("Car"));
            Assert.That(_flexibleEmpl.DeliveryArea.Equals("Downtown"));
            Assert.That(_flexibleEmpl.TipsEarned.Equals(100.00));
            Assert.That(_flexibleEmpl.Status.Equals(StatusEmpl.FullTime));
            Assert.That(_flexibleEmpl.EmployeeType.Equals("FlexibleEmpl"));
        }

        [Test]
        public void Vehicle_SetWithNullOrWhiteSpace_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _flexibleEmpl.Vehicle = "");
            Assert.That(ex.Message.Equals("Vehicle cannot be null or empty (Parameter 'Vehicle')"));
        }

        [Test]
        public void DeliveryArea_SetWithNullOrWhiteSpace_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _flexibleEmpl.DeliveryArea = "");
            Assert.That(ex.Message.Equals("Delivery area cannot be null or empty (Parameter 'DeliveryArea')"));
        }

        [Test]
        public void TipsEarned_SetWithNegativeValue_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _flexibleEmpl.TipsEarned = -50);
            Assert.That(ex.Message.Equals("Tips earned cannot be negative (Parameter 'TipsEarned')"));
        }
        
        [Test]
        public void Constructor_WithInvalidVehicle_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new FlexibleEmpl(
                    id: 1,
                    name: "John Doe",
                    salary: 3000.00,
                    hireDate: DateTime.Now.AddYears(-2),
                    shiftStart: DateTime.Now.AddHours(-5),
                    shiftEnd: DateTime.Now,
                    status: StatusEmpl.FullTime,
                    isBranchManager: false,
                    vehicle: "",  // Invalid
                    deliveryArea: "Downtown",
                    tipsEarned: 100.00
                )
            );
            Assert.That(ex.Message.Equals("Vehicle cannot be null or empty (Parameter 'Vehicle')"));
        }

        [Test]
        public void Constructor_WithInvalidDeliveryArea_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new FlexibleEmpl(
                    id: 1,
                    name: "John Doe",
                    salary: 3000.00,
                    hireDate: DateTime.Now.AddYears(-2),
                    shiftStart: DateTime.Now.AddHours(-5),
                    shiftEnd: DateTime.Now,
                    status: StatusEmpl.FullTime,
                    isBranchManager: false,
                    vehicle: "Car",
                    deliveryArea: "",  // Invalid
                    tipsEarned: 100.00
                )
            );
            Assert.That(ex.Message.Equals("Delivery area cannot be null or empty (Parameter 'DeliveryArea')"));
        }

        [Test]
        public void Constructor_WithNegativeTipsEarned_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new FlexibleEmpl(
                    id: 1,
                    name: "John Doe",
                    salary: 3000.00,
                    hireDate: DateTime.Now.AddYears(-2),
                    shiftStart: DateTime.Now.AddHours(-5),
                    shiftEnd: DateTime.Now,
                    status: StatusEmpl.FullTime,
                    isBranchManager: false,
                    vehicle: "Car",
                    deliveryArea: "Downtown",
                    tipsEarned: -100.00  // Invalid
                )
            );
            Assert.That(ex.Message.Equals("Tips earned cannot be negative (Parameter 'TipsEarned')"));
        }
    }
}
