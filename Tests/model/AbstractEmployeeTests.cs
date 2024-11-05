using CashInn.Enum;
using CashInn.Model.Employee;

namespace Tests.model;

[TestFixture]
public class AbstractEmployeeTests
{
    private Cook _cook = null!;
    [SetUp]
    public void SetUp()
    {
        _cook = new Cook(
            1,
            "Test Cook",
            30000,
            DateTime.Now.AddYears(-1),
            DateTime.Today.AddHours(9),
            DateTime.Today.AddHours(17),
            StatusEmpl.FullTime,
            false,
            "Italian",
            5,
            "Main Kitchen"
        );
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _cook.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _cook.Id = 2;
        Assert.That(_cook.Id, Is.EqualTo(2));
    }

    [Test]
    public void Name_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _cook.Name = null);
    }

    [Test]
    public void Name_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _cook.Name = "  ");
    }

    [Test]
    public void Name_SetNonWhiteSpace_ShouldSet()
    {
        _cook.Name = "Valid Name";
        Assert.That(_cook.Name, Is.EqualTo("Valid Name"));
    }

    [Test]
    public void Salary_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _cook.Salary = -10000);
    }

    [Test]
    public void Salary_SetPositiveValue_ShouldSet()
    {
        _cook.Salary = 45000;
        Assert.That(_cook.Salary, Is.EqualTo(45000));
    }

    [Test]
    public void HireDate_SetFutureDate_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _cook.HireDate = DateTime.Now.AddDays(1));
    }

    [Test]
    public void HireDate_SetPastDate_ShouldSet()
    {
        var pastDate = DateTime.Now.AddYears(-2);
        _cook.HireDate = pastDate;
        Assert.That(_cook.HireDate, Is.EqualTo(pastDate));
    }

    [Test]
    public void LayoffDate_SetBeforeHireDateAndHasValue_ShouldThrowException()
    {
        _cook.HireDate = DateTime.Now.AddYears(-1);
        var invalidLayoffDate = _cook.HireDate.AddDays(-1);
        Assert.Throws<ArgumentException>(() => _cook.LayoffDate = invalidLayoffDate);
    }

    [Test]
    public void LayoffDate_SetAfterHireDateAndHasValue_ShouldSet()
    {
        var validLayoffDate = _cook.HireDate.AddMonths(6);
        _cook.LayoffDate = validLayoffDate;
        Assert.That(_cook.LayoffDate, Is.EqualTo(validLayoffDate));
    }

    [Test]
    public void LayoffDate_SetNull_ShouldSet()
    {
        _cook.LayoffDate = null;
        Assert.IsNull(_cook.LayoffDate);
    }

    [Test]
    public void ShiftStart_SetAfterShiftEnd_ShouldThrowException()
    {
        // Temporarily set ShiftStart to a valid time before ShiftEnd
        _cook.ShiftStart = DateTime.Today.AddHours(7);
        
        _cook.ShiftEnd = DateTime.Today.AddHours(8);
        
        Assert.Throws<ArgumentException>(() => _cook.ShiftStart = DateTime.Today.AddHours(10));
    }

    [Test]
    public void ShiftStart_SetBeforeShiftEnd_ShouldSet()
    {
        _cook.ShiftEnd = DateTime.Today.AddHours(10);
        
        var shiftStart = DateTime.Today.AddHours(8);
        _cook.ShiftStart = shiftStart;

        Assert.That(_cook.ShiftStart, Is.EqualTo(shiftStart));
    }

    [Test]
    public void ShiftEnd_SetBeforeShiftStart_ShouldThrowException()
    {
        _cook.ShiftStart = DateTime.Today.AddHours(8);
        Assert.Throws<ArgumentException>(() => _cook.ShiftEnd = DateTime.Today.AddHours(7));
    }

    [Test]
    public void ShiftEnd_SetAfterShiftStart_ShouldSet()
    {
        var shiftEnd = DateTime.Today.AddHours(18);
        _cook.ShiftStart = DateTime.Today.AddHours(8);
        _cook.ShiftEnd = shiftEnd;
        Assert.That(_cook.ShiftEnd, Is.EqualTo(shiftEnd));
    }
}