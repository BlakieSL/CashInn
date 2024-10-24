using NUnit.Framework;
using CashInn.Model;
using System;
using System.Collections.Generic;

namespace CashInn.Test
{
    public class ReservationTests
    {
        private Reservation _reservation;

        [SetUp]
        public void Setup()
        {
            _reservation = new Reservation
            {
                Id = 1,
                Name = "John Doe",
                Address = "123 Main St",
                Email = "johndoe@example.com",
                ContactNumber = "123-456-7890"
            };
        }

        [Test]
        public void Constructor_ShouldInitializeCorrectly()
        {
            Assert.That(_reservation.Id.Equals(1));
            Assert.That(_reservation.Name.Equals("John Doe"));
            Assert.That(_reservation.Address.Equals("123 Main St"));
            Assert.That(_reservation.Email.Equals("johndoe@example.com"));
            Assert.That(_reservation.ContactNumber.Equals("123-456-7890"));
        }

        [Test]
        public void SetName_ShouldThrowArgumentException_WhenNameIsNullOrEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => _reservation.Name = "");
            Assert.That(ex.Message.Equals("Name cannot be null or empty (Parameter 'Name')"));
        }

        [Test]
        public void SetAddress_ShouldThrowArgumentException_WhenAddressIsNullOrEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => _reservation.Address = "");
            Assert.That(ex.Message.Equals("Address info cannot be null or empty"));
        }

        [Test]
        public void SetEmail_ShouldThrowArgumentException_WhenEmailIsNullOrEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => _reservation.Email = "");
            Assert.That(ex.Message.Equals("Email cannot be null or empty"));
        }

        [Test]
        public void SaveExtent_ShouldPersistReservations()
        {
            var filePath = "reservations.json";
            Reservation.SaveReservation(_reservation);
            Reservation.SaveExtent(filePath);
            Assert.That(System.IO.File.Exists(filePath));
        }
    }
}
