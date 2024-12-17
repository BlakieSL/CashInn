using CashInn.Model;
using NUnit.Framework;
using System;
using System.Linq;

namespace Tests.Association
{
    [TestFixture]
    [TestOf(typeof(TableReservationAssociation))]
    public class TableReservationAssociationTest
    {
        private Table _table1 = null!;
        private Table _table2 = null!;
        private Reservation _reservation1 = null!;
        private Reservation _reservation2 = null!;
        private Branch _branch = null!;
        private Customer _customer = null!;

        [SetUp]
        public void SetUp()
        {
            _branch = new Branch(1, "Main Branch", "random");
            _customer = new Customer(1, "John Doe", null, "123 Main St", "john@example.com");

            _table1 = new Table(1, 4, _branch);
            _table2 = new Table(2, 6, _branch);

            _reservation1 = new Reservation(1, 4, _customer);
            _reservation2 = new Reservation(2, 6, _customer);
        }

        [Test]
        public void AddReservationToTable_ShouldCreateBidirectionalAssociation()
        {
            _table1.AddReservation(_reservation1, DateTime.Now, DateTime.Now.AddHours(2));

            Assert.Multiple(() =>
            {
                Assert.That(_table1.ReservationAssociations.Count(), Is.EqualTo(1));
                Assert.That(_reservation1.TableAssociations.Count(), Is.EqualTo(1));
                Assert.That(_table1.ReservationAssociations.First().Reservation, Is.EqualTo(_reservation1));
                Assert.That(_reservation1.TableAssociations.First().Table, Is.EqualTo(_table1));
            });
        }

        [Test]
        public void AddReservationToTable_WhenSameReservationMultipleTimes_ShouldAllowMultipleEntries()
        {
            _table1.AddReservation(_reservation1, DateTime.Now, DateTime.Now.AddHours(2));
            _table1.AddReservation(_reservation1, DateTime.Now.AddHours(3), DateTime.Now.AddHours(5));

            Assert.That(_table1.ReservationAssociations.Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddReservationToTable_WhenEndTimeBeforeStartTime_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                _table1.AddReservation(_reservation1, DateTime.Now, DateTime.Now.AddHours(-1)));
        }

        [Test]
        public void RemoveReservationFromTable_ShouldRemoveBidirectionalAssociation()
        {
            _table1.AddReservation(_reservation1, DateTime.Now, DateTime.Now.AddHours(2));
            _table1.RemoveReservation(_reservation1);

            Assert.Multiple(() =>
            {
                Assert.That(_table1.ReservationAssociations, Has.None.Matches<TableReservationAssociation>(
                    a => a.Reservation == _reservation1));
                Assert.That(_reservation1.TableAssociations, Has.None.Matches<TableReservationAssociation>(
                    a => a.Table == _table1));
            });
        }

        [Test]
        public void RemoveReservationFromTable_WhenReservationNotAssociated_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => _table1.RemoveReservation(_reservation1));
        }

        [Test]
        public void AddTableToReservation_ShouldCreateBidirectionalAssociation()
        {
            _reservation1.AddTable(_table1, DateTime.Now, DateTime.Now.AddHours(2));

            Assert.Multiple(() =>
            {
                Assert.That(_reservation1.TableAssociations.Count(), Is.EqualTo(1));
                Assert.That(_table1.ReservationAssociations.Count(), Is.EqualTo(1));
                Assert.That(_reservation1.TableAssociations.First().Table, Is.EqualTo(_table1));
                Assert.That(_table1.ReservationAssociations.First().Reservation, Is.EqualTo(_reservation1));
            });
        }

        [Test]
        public void RemoveTableFromReservation_ShouldRemoveBidirectionalAssociation()
        {
            _reservation1.AddTable(_table1, DateTime.Now, DateTime.Now.AddHours(2));
            _reservation1.RemoveTable(_table1);

            Assert.Multiple(() =>
            {
                Assert.That(_reservation1.TableAssociations, Has.None.Matches<TableReservationAssociation>(
                    a => a.Table == _table1));
                Assert.That(_table1.ReservationAssociations, Has.None.Matches<TableReservationAssociation>(
                    a => a.Reservation == _reservation1));
            });
        }

        [Test]
        public void AddMultipleReservationsToTable_ShouldCreateMultipleAssociations()
        {
            _table1.AddReservation(_reservation1, DateTime.Now, DateTime.Now.AddHours(2));
            _table1.AddReservation(_reservation2, DateTime.Now.AddHours(3), DateTime.Now.AddHours(5));

            Assert.Multiple(() =>
            {
                Assert.That(_table1.ReservationAssociations.Count(), Is.EqualTo(2));
                Assert.That(_reservation1.TableAssociations.Count(), Is.EqualTo(1));
                Assert.That(_reservation2.TableAssociations.Count(), Is.EqualTo(1));
            });
        }

        [Test]
        public void AddMultipleTablesToReservation_ShouldCreateMultipleAssociations()
        {
            _reservation1.AddTable(_table1, DateTime.Now, DateTime.Now.AddHours(2));
            _reservation1.AddTable(_table2, DateTime.Now.AddHours(3), DateTime.Now.AddHours(5));

            Assert.Multiple(() =>
            {
                Assert.That(_reservation1.TableAssociations.Count(), Is.EqualTo(2));
                Assert.That(_table1.ReservationAssociations.Count(), Is.EqualTo(1));
                Assert.That(_table2.ReservationAssociations.Count(), Is.EqualTo(1));
            });
        }

        [Test]
        public void RemoveReservationFromTable_ShouldNotAffectOtherAssociations()
        {
            _table1.AddReservation(_reservation1, DateTime.Now, DateTime.Now.AddHours(2));
            _table1.AddReservation(_reservation2, DateTime.Now.AddHours(3), DateTime.Now.AddHours(5));

            _table1.RemoveReservation(_reservation1);

            Assert.Multiple(() =>
            {
                Assert.That(_table1.ReservationAssociations,
                    Contains.Item(_table1.ReservationAssociations.FirstOrDefault(a => a.Reservation == _reservation2)));
                Assert.That(_reservation2.TableAssociations,
                    Contains.Item(_reservation2.TableAssociations.FirstOrDefault(a => a.Table == _table1)));
                Assert.That(_reservation1.TableAssociations, Is.Empty);
            });
        }

        [Test]
        public void RemoveTableFromReservation_ShouldNotAffectOtherAssociations()
        {
            _reservation1.AddTable(_table1, DateTime.Now, DateTime.Now.AddHours(2));
            _reservation1.AddTable(_table2, DateTime.Now.AddHours(3), DateTime.Now.AddHours(5));

            _reservation1.RemoveTable(_table1);

            Assert.Multiple(() =>
            {
                Assert.That(_reservation1.TableAssociations,
                    Contains.Item(_reservation1.TableAssociations.FirstOrDefault(a => a.Table == _table2)));
                Assert.That(_table2.ReservationAssociations,
                    Contains.Item(_table2.ReservationAssociations.FirstOrDefault(a => a.Reservation == _reservation1)));
                Assert.That(_table1.ReservationAssociations, Is.Empty);
            });
        }

        [Test]
        public void UpdateStartDateTime_ShouldUpdateStartDateTimeSuccessfully()
        {
            DateTime initialStartTime = new DateTime(2024, 12, 17, 13, 52, 42);
            DateTime initialEndTime = initialStartTime.AddHours(2);
            DateTime newStartTime = initialStartTime.AddHours(1);

            TableReservationAssociation association =
                new TableReservationAssociation(_table1, _reservation1, initialStartTime, initialEndTime);

            association.UpdateStartDateTime(newStartTime);

            Assert.That(association.StartDateTime, Is.EqualTo(newStartTime));
        }

        [Test]
        public void UpdateEndDateTime_ShouldUpdateEndDateTimeSuccessfully()
        {
            DateTime initialStartTime = new DateTime(2024, 12, 17, 13, 52, 42);
            DateTime initialEndTime = initialStartTime.AddHours(2);
            DateTime newEndTime = initialEndTime.AddHours(1);

            TableReservationAssociation association =
                new TableReservationAssociation(_table1, _reservation1, initialStartTime, initialEndTime);

            association.UpdateEndDateTime(newEndTime);

            Assert.That(association.EndDateTime, Is.EqualTo(newEndTime));
        }

        [Test]
        public void UpdateStartDateTime_WhenNewStartDateTimeIsAfterEndDateTime_ShouldThrowException()
        {
            DateTime initialStartTime = new DateTime(2024, 12, 17, 13, 52, 42);
            DateTime initialEndTime = initialStartTime.AddHours(2);
            DateTime newStartTime = initialEndTime.AddHours(1);

            TableReservationAssociation association =
                new TableReservationAssociation(_table1, _reservation1, initialStartTime, initialEndTime);
            Assert.Throws<ArgumentException>(() => association.UpdateStartDateTime(newStartTime));
        }

        [Test]
        public void UpdateEndDateTime_WhenNewEndDateTimeIsBeforeStartDateTime_ShouldThrowException()
        {
            DateTime initialStartTime = new DateTime(2024, 12, 17, 13, 52, 42);
            DateTime initialEndTime = initialStartTime.AddHours(2);
            DateTime newEndTime = initialStartTime.AddHours(-1);

            TableReservationAssociation association =
                new TableReservationAssociation(_table1, _reservation1, initialStartTime, initialEndTime);
            Assert.Throws<ArgumentException>(() => association.UpdateEndDateTime(newEndTime));
        }
    }
}