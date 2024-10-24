using NUnit.Framework;
using CashInn.Model;
using CashInn.Enum;
using System;
using System.IO;
using System.Collections.Immutable;

namespace CashInn.Test
{
    public class AbstractEmployeeTests
    {
        private string _filePath;
        private Waiter _waiter;
        private FlexibleEmpl _flexibleEmpl;

        [SetUp]
        public void Setup()
        {
            _filePath = Path.Combine(Path.GetTempPath(), "employees.json");

            _waiter = new Waiter(
                id: 1,
                name: "John Doe",
                salary: 1500.00,
                hireDate: DateTime.Now.AddYears(-1),
                shiftStart: DateTime.Now.AddHours(-5),
                shiftEnd: DateTime.Now,
                status: StatusEmpl.PartTime,
                isBranchManager: false,
                tipsEarned: 200.00
            );

            _flexibleEmpl = new FlexibleEmpl(
                id: 2,
                name: "Jane Smith",
                salary: 2500.00,
                hireDate: DateTime.Now.AddYears(-2),
                shiftStart: DateTime.Now.AddHours(-6),
                shiftEnd: DateTime.Now.AddHours(-1),
                status: StatusEmpl.FullTime,
                isBranchManager: false,
                vehicle: "Scooter",
                deliveryArea: "City Center",
                tipsEarned: 300.00
            );

            AbstractEmployee.SaveEmployee(_waiter);
            AbstractEmployee.SaveEmployee(_flexibleEmpl);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }
        
        [Test]
        public void SaveExtent_ShouldSerializeEmployeesToFile()
        {
            AbstractEmployee.SaveExtent(_filePath);
            
            Assert.That(File.Exists(_filePath).Equals(true));

            string json = File.ReadAllText(_filePath);
            Assert.That(!string.IsNullOrEmpty(json).Equals(true));
            Assert.That(json.Contains("John Doe").Equals(true));
            Assert.That(json.Contains("Jane Smith").Equals(true));
        }

        [Test]
        public void Salary_SetNegativeValue_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _waiter.Salary = -500);
            Assert.That(ex.Message.Equals("Salary cannot be negative (Parameter 'Salary')"));
        }

        [Test]
        public void HireDate_SetInFuture_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _waiter.HireDate = DateTime.Now.AddDays(1));
            Assert.That(ex.Message.Equals("Hire date cannot be in the future (Parameter 'HireDate')"));
        }

        [Test]
        public void LayoffDate_BeforeHireDate_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _waiter.LayoffDate = _waiter.HireDate.AddDays(-1));
            Assert.That(ex.Message.Equals("Layoff date cannot be before hire date (Parameter 'LayoffDate')"));
        }

        [Test]
        public void ShiftStart_AfterShiftEnd_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _waiter.ShiftStart = _waiter.ShiftEnd.AddHours(1));
            Assert.That(ex.Message.Equals("Shift start time cannot be after shift end time (Parameter 'ShiftStart')"));
        }

        [Test]
        public void ShiftEnd_BeforeShiftStart_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _waiter.ShiftEnd = _waiter.ShiftStart.AddHours(-1));
            Assert.That(ex.Message.Equals("Shift end time cannot be before shift start time (Parameter 'ShiftEnd')"));
        }
    }
}
