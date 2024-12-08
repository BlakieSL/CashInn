using CashInn.Enum;
using CashInn.Model;
using CashInn.Model.Employee;

namespace Tests.model;

[TestFixture]
public class AbstractEmployeeTests
{
    private Cook _cook = null!;
    private Chef _chef = null!;
    private Branch _branch = null!;
    private const string TestFilePath = "TestEmployees.json";

    [SetUp]
    public void SetUp()
    {
        AbstractEmployee.ClearExtent();
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }
        typeof(AbstractEmployee)
            .GetField("_filepath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .SetValue(null, TestFilePath);

        _branch = new Branch(1, "ul.Hermana", "+485757575");
        
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
            "Main Kitchen",
            _branch
        );

        _chef = new Chef(
            2,
            "Test Chef",
            45000,
            DateTime.Now.AddYears(-2),
            DateTime.Today.AddHours(10),
            DateTime.Today.AddHours(18),
            StatusEmpl.PartTime,
            true,
            "French",
            10,
            2,
            _branch
        );

    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }
        AbstractEmployee.ClearExtent();
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
        Assert.Throws<ArgumentException>(() => _cook.Name = null!);
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

    [Test]
    public void SaveEmployee_ShouldAddEmployeeToCollection()
    {
        AbstractEmployee.ClearExtent();
        AbstractEmployee.SaveEmployee(_cook);

        var employees = AbstractEmployee.GetAll();
        Assert.That(employees, Has.Count.EqualTo(1));
        Assert.That(employees.First(), Is.EqualTo(_cook));
    }

    [Test]
    public void GetAll_ShouldReturnImmutableListOfEmployees()
    {
        AbstractEmployee.ClearExtent();
        AbstractEmployee.SaveEmployee(_cook);
        AbstractEmployee.SaveEmployee(_chef);

        var employees = AbstractEmployee.GetAll();

        Assert.That(employees, Has.Count.EqualTo(2));
        Assert.That(employees, Contains.Item(_cook));
        Assert.That(employees, Contains.Item(_chef));
        Assert.That(() => (employees).Add(_cook), Throws.TypeOf<NotSupportedException>());
    }

    [Test]
    public void ClearExtent_ShouldRemoveAllEmployees()
    {
        AbstractEmployee.SaveEmployee(_cook);
        AbstractEmployee.SaveEmployee(_chef);

        AbstractEmployee.ClearExtent();

        var employees = AbstractEmployee.GetAll();
        Assert.That(employees, Is.Empty);
    }

    [Test]
    public void SaveExtent_ShouldWriteEmployeesToFile()
    {
        AbstractEmployee.ClearExtent();
        AbstractEmployee.SaveEmployee(_cook);
        AbstractEmployee.SaveEmployee(_chef);

        AbstractEmployee.SaveExtent();

        Assert.That(File.Exists(TestFilePath), Is.True);
        var fileContent = File.ReadAllText(TestFilePath);
        Assert.That(fileContent, Does.Contain("Test Cook"));
        Assert.That(fileContent, Does.Contain("Test Chef"));
    }

    [Test]
    public void LoadExtent_ShouldRetrieveEmployeesFromFile()
    {
        AbstractEmployee.ClearExtent();

        AbstractEmployee.SaveEmployee(_cook);
        AbstractEmployee.SaveEmployee(_chef);
        AbstractEmployee.SaveExtent();

        // Clear the in-memory collection
        AbstractEmployee.ClearExtent();

        AbstractEmployee.LoadExtent();

        var employees = AbstractEmployee.GetAll();
        Assert.That(employees, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(employees.Any(e => e.Name == "Test Cook"), Is.True);
            Assert.That(employees.Any(e => e.Name == "Test Chef"), Is.True);
        });
    }

    [Test]
    public void LoadExtent_WithInvalidFile_ShouldNotThrowException()
    {
        File.WriteAllText(TestFilePath, "Invalid JSON Content");

        Assert.DoesNotThrow(AbstractEmployee.LoadExtent);
        Assert.That(AbstractEmployee.GetAll(), Is.Empty);
    }
}