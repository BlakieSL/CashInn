using CashInn.Model;

namespace Tests.model;

[TestFixture]
[TestOf(typeof(Reservation))]
public class ReservationTest
{
    private Reservation _reservation = null!;
    private const string TestFilePath = "Reservations.json";

    [SetUp]
    public void SetUp()
    {
        _reservation = new Reservation(1, 4);
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

        Reservation.ClearExtent();
        Reservation.LoadExtent();
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _reservation.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _reservation.Id = 2;
        Assert.That(_reservation.Id, Is.EqualTo(2));
    }

    [Test]
    public void NumberOfGuests_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _reservation.NumberOfGuests = -1);
    }

    [Test]
    public void NumberOfGuests_SetPositiveValue_ShouldSet()
    {
        _reservation.NumberOfGuests = 5;
        Assert.That(_reservation.NumberOfGuests, Is.EqualTo(5));
    }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredReservationsCorrectly()
    {
        var reservation1 = new Reservation(1, 4);
        var reservation2 = new Reservation(2, 6);

        Reservation.SaveExtent();

        Reservation.LoadExtent();
        Assert.AreEqual(2, Reservation.GetAll().Count);

        reservation1 = null!;
        reservation2 = null!;
        Reservation.LoadExtent();

        Assert.AreEqual(2, Reservation.GetAll().Count);

        var loadedReservations = Reservation.GetAll();
        Assert.IsTrue(loadedReservations.Any(r => r.Id == 1));
        Assert.IsTrue(loadedReservations.Any(r => r.Id == 2));
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }
    }
}