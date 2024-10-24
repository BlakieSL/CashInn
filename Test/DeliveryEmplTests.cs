using NUnit.Framework;
using CashInn.Model;
using CashInn.Enum;
using System;

namespace CashInn.Test
{
    public class DeliveryEmplTests
    {
        private DeliveryEmpl _deliveryEmployee;

        [SetUp]
        public void Setup()
        {
            _deliveryEmployee = new DeliveryEmpl(
                id: 1,
                name: "John Doe",
                salary: 2000.00,
                hireDate: DateTime.Now.AddYears(-1),
                shiftStart: DateTime.Now.AddHours(-5),
                shiftEnd: DateTime.Now,
                status: StatusEmpl.FullTime,
                isBranchManager: false,
                vehicle: "Car",
                deliveryArea: "Downtown"
            );
        }

        [Test]
        public void Constructor_ShouldInitializeCorrectly()
        {
            Assert.That(_deliveryEmployee.Id.Equals(1));
            Assert.That(_deliveryEmployee.Name.Equals("John Doe"));
            Assert.That(_deliveryEmployee.Salary.Equals(2000.00));
            Assert.That(_deliveryEmployee.Vehicle.Equals("Car"));
            Assert.That(_deliveryEmployee.DeliveryArea.Equals("Downtown"));
            Assert.That(_deliveryEmployee.EmployeeType.Equals("DeliveryEmpl"));
            Assert.That(_deliveryEmployee.Status.Equals(StatusEmpl.FullTime));
        }

        [Test]
        public void SetVehicle_ShouldThrowArgumentException_WhenVehicleIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => _deliveryEmployee.Vehicle = "");
            Assert.That(ex.Message.Equals("Vehicle cannot be null or empty (Parameter 'Vehicle')"));
        }

        [Test]
        public void SetDeliveryArea_ShouldThrowArgumentException_WhenDeliveryAreaIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => _deliveryEmployee.DeliveryArea = "");
            Assert.That(ex.Message.Equals("Delivery area cannot be null or empty (Parameter 'DeliveryArea')"));
        }

        [Test]
        public void SetVehicle_ShouldUpdateValue_WhenValidInput()
        {
            _deliveryEmployee.Vehicle = "Bike";

            Assert.That(_deliveryEmployee.Vehicle.Equals("Bike"));
        }

        [Test]
        public void SetDeliveryArea_ShouldUpdateValue_WhenValidInput()
        {
            _deliveryEmployee.DeliveryArea = "Suburbs";

            Assert.That(_deliveryEmployee.DeliveryArea.Equals("Suburbs"));
        }

        [Test]
        public void HireDate_ShouldThrowArgumentException_WhenHireDateIsInFuture()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new DeliveryEmpl(2, "Jane Doe", 1500.00, DateTime.Now.AddYears(1), DateTime.Now.AddHours(-4),
                    DateTime.Now, StatusEmpl.FullTime, false, "Bike", "Midtown"));
            Assert.That(ex.Message.Equals("Hire date cannot be in the future (Parameter 'HireDate')"));
        }

        [Test]
        public void ShiftTimes_ShouldThrowArgumentException_WhenShiftStartIsAfterShiftEnd()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new DeliveryEmpl(3, "Sam Smith", 1800.00, DateTime.Now.AddYears(-2), DateTime.Now.AddHours(1),
                    DateTime.Now.AddHours(-1), StatusEmpl.FullTime, false, "Scooter", "Uptown"));
            Assert.That(ex.Message.Equals("Shift start time cannot be after shift end time (Parameter 'ShiftStart')"));
        }
    }
}
