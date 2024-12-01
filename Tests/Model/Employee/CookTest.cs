using CashInn.Enum;
using CashInn.Model.Employee;

namespace Tests.model.Employee;

[TestFixture]
[TestOf(typeof(Cook))]
public class CookTest
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
    public void SpecialtyCuisine_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _cook.SpecialtyCuisine = null);
    }

    [Test]
    public void SpecialtyCuisine_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _cook.SpecialtyCuisine = "  ");
    }

    [Test]
    public void SpecialtyCuisine_SetValidValue_ShouldSet()
    {
        _cook.SpecialtyCuisine = "French";
        Assert.That(_cook.SpecialtyCuisine, Is.EqualTo("French"));
    }

    [Test]
    public void YearsOfExperience_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _cook.YearsOfExperience = -1);
    }

    [Test]
    public void YearsOfExperience_SetValidValue_ShouldSet()
    {
        _cook.YearsOfExperience = 10;
        Assert.That(_cook.YearsOfExperience, Is.EqualTo(10));
    }

    [Test]
    public void Station_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _cook.Station = null);
    }

    [Test]
    public void Station_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _cook.Station = "  ");
    }

    [Test]
    public void Station_SetValidValue_ShouldSet()
    {
        _cook.Station = "Grill Station";
        Assert.That(_cook.Station, Is.EqualTo("Grill Station"));
    }

    [Test]
    public void ToSerializableObject_ShouldReturnExpectedObject()
    {
        var serializableObject = _cook.ToSerializableObject();
        
        Assert.Multiple(() =>
        {
            Assert.That(serializableObject, Has.Property("Id").EqualTo(_cook.Id));
            Assert.That(serializableObject, Has.Property("Name").EqualTo(_cook.Name));
            Assert.That(serializableObject, Has.Property("Salary").EqualTo(_cook.Salary));
            Assert.That(serializableObject, Has.Property("HireDate").EqualTo(_cook.HireDate));
            Assert.That(serializableObject, Has.Property("ShiftStart").EqualTo(_cook.ShiftStart));
            Assert.That(serializableObject, Has.Property("ShiftEnd").EqualTo(_cook.ShiftEnd));
            Assert.That(serializableObject, Has.Property("Status").EqualTo(_cook.Status));
            Assert.That(serializableObject, Has.Property("IsBranchManager").EqualTo(_cook.IsBranchManager));
            Assert.That(serializableObject, Has.Property("LayoffDate").EqualTo(_cook.LayoffDate));
            Assert.That(serializableObject, Has.Property("SpecialtyCuisine").EqualTo(_cook.SpecialtyCuisine));
            Assert.That(serializableObject, Has.Property("YearsOfExperience").EqualTo(_cook.YearsOfExperience));
            Assert.That(serializableObject, Has.Property("Station").EqualTo(_cook.Station));
            Assert.That(serializableObject, Has.Property("EmployeeType").EqualTo(_cook.EmployeeType));
        });
    }
}