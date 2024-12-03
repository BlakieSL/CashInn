using CashInn.Enum;
using CashInn.Model.Employee;

namespace Tests.model.Employee;

[TestFixture]
[TestOf(typeof(Chef))]
public class ChefTest
{
private Chef _chef = null!;
    [SetUp]
    public void SetUp()
    {
        _chef = new Chef(
            1,
            "Test Chef",
            50000,
            DateTime.Now.AddYears(-5),
            DateTime.Today.AddHours(10),
            DateTime.Today.AddHours(18),
            StatusEmpl.FullTime,
            false,
            "French",
            10,
            2
        );
    }

    [Test]
    public void SpecialtyCuisine_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _chef.SpecialtyCuisine = null);
    }

    [Test]
    public void SpecialtyCuisine_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _chef.SpecialtyCuisine = "  ");
    }

    [Test]
    public void SpecialtyCuisine_SetValid_ShouldSet()
    {
        _chef.SpecialtyCuisine = "Italian";
        Assert.That(_chef.SpecialtyCuisine, Is.EqualTo("Italian"));
    }

    [Test]
    public void YearsOfExperience_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _chef.YearsOfExperience = -1);
    }

    [Test]
    public void YearsOfExperience_SetPositiveValue_ShouldSet()
    {
        _chef.YearsOfExperience = 8;
        Assert.That(_chef.YearsOfExperience, Is.EqualTo(8));
    }

    [Test]
    public void YearsOfExperience_SetPositiveValue_ShouldCalculateBonus()
    {
        var expectedBonus = 8 * 0.04 * _chef.Salary;
        _chef.YearsOfExperience = 8;
        Assert.That(_chef.ExperienceBonus, Is.EqualTo(expectedBonus).Within(0.01));
    }

    [Test]
    public void MichelinStars_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _chef.MichelinStars = -1);
    }

    [Test]
    public void MichelinStars_SetAboveLimit_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _chef.MichelinStars = 4);
    }

    [Test]
    public void MichelinStars_SetWithinLimit_ShouldSet()
    {
        _chef.MichelinStars = 3;
        Assert.That(_chef.MichelinStars, Is.EqualTo(3));
    }
    
    [Test]
    public void ToSerializableObject_ShouldReturnExpectedObject()
    {
        var serializableObject = _chef.ToSerializableObject();
        
        Assert.Multiple(() =>
        {
            Assert.That(serializableObject, Has.Property("Id").EqualTo(_chef.Id));
            Assert.That(serializableObject, Has.Property("Name").EqualTo(_chef.Name));
            Assert.That(serializableObject, Has.Property("Salary").EqualTo(_chef.Salary));
            Assert.That(serializableObject, Has.Property("HireDate").EqualTo(_chef.HireDate));
            Assert.That(serializableObject, Has.Property("ShiftStart").EqualTo(_chef.ShiftStart));
            Assert.That(serializableObject, Has.Property("ShiftEnd").EqualTo(_chef.ShiftEnd));
            Assert.That(serializableObject, Has.Property("Status").EqualTo(_chef.Status));
            Assert.That(serializableObject, Has.Property("IsBranchManager").EqualTo(_chef.IsBranchManager));
            Assert.That(serializableObject, Has.Property("LayoffDate").EqualTo(_chef.LayoffDate));
            Assert.That(serializableObject, Has.Property("SpecialtyCuisine").EqualTo(_chef.SpecialtyCuisine));
            Assert.That(serializableObject, Has.Property("YearsOfExperience").EqualTo(_chef.YearsOfExperience));
            Assert.That(serializableObject, Has.Property("MichelinStars").EqualTo(_chef.MichelinStars));
            Assert.That(serializableObject, Has.Property("EmployeeType").EqualTo(_chef.EmployeeType));
        });
    }
}