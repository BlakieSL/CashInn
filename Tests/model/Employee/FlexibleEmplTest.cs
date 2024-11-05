using CashInn.Enum;
using CashInn.Model.Employee;

namespace Tests.model.Employee;

[TestFixture]
[TestOf(typeof(FlexibleEmpl))]
public class FlexibleEmplTest
{
    private FlexibleEmpl _flexibleEmpl = null!;
    
    [SetUp]
    public void SetUp()
    {
        _flexibleEmpl = new FlexibleEmpl(
            2,
            "Test Flexible Employee",
            35000,
            DateTime.Now.AddYears(-2),
            DateTime.Today.AddHours(10),
            DateTime.Today.AddHours(18),
            StatusEmpl.PartTime,
            false,
            "Car",
            "Suburb Area",
            150.0
        );
    }

    [Test]
    public void Vehicle_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _flexibleEmpl.Vehicle = null);
    }
    
    [Test]
    public void Vehicle_Empty_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _flexibleEmpl.Vehicle = "   ");
    }

    [Test]
    public void Vehicle_SetNonNull_ShouldSet()
    {
        _flexibleEmpl.Vehicle = "Van";
        Assert.That(_flexibleEmpl.Vehicle, Is.EqualTo("Van"));
    }

    [Test]
    public void DeliveryArea_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _flexibleEmpl.DeliveryArea = null);
    }
    
    [Test]
    public void DeliveryArea_Empty_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _flexibleEmpl.DeliveryArea = null);
    }

    [Test]
    public void DeliveryArea_SetNonNull_ShouldSet()
    {
        _flexibleEmpl.DeliveryArea = "City Center";
        Assert.That(_flexibleEmpl.DeliveryArea, Is.EqualTo("City Center"));
    }

    [Test]
    public void TipsEarned_SetNegative_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _flexibleEmpl.TipsEarned = -1.0);
    }

    [Test]
    public void TipsEarned_SetPositive_ShouldSet()
    {
        _flexibleEmpl.TipsEarned = 200.0;
        Assert.That(_flexibleEmpl.TipsEarned, Is.EqualTo(200.0));
    }
    
    [Test]
    public void ToSerializableObject_ShouldReturnExpectedObject()
    {
        var serializableObject = _flexibleEmpl.ToSerializableObject();
        
        Assert.Multiple(() =>
        {
            Assert.That(serializableObject, Has.Property("Id").EqualTo(_flexibleEmpl.Id));
            Assert.That(serializableObject, Has.Property("Name").EqualTo(_flexibleEmpl.Name));
            Assert.That(serializableObject, Has.Property("Salary").EqualTo(_flexibleEmpl.Salary));
            Assert.That(serializableObject, Has.Property("HireDate").EqualTo(_flexibleEmpl.HireDate));
            Assert.That(serializableObject, Has.Property("ShiftStart").EqualTo(_flexibleEmpl.ShiftStart));
            Assert.That(serializableObject, Has.Property("ShiftEnd").EqualTo(_flexibleEmpl.ShiftEnd));
            Assert.That(serializableObject, Has.Property("Status").EqualTo(_flexibleEmpl.Status));
            Assert.That(serializableObject, Has.Property("IsBranchManager").EqualTo(_flexibleEmpl.IsBranchManager));
            Assert.That(serializableObject, Has.Property("LayoffDate").EqualTo(_flexibleEmpl.LayoffDate));
            Assert.That(serializableObject, Has.Property("Vehicle").EqualTo(_flexibleEmpl.Vehicle));
            Assert.That(serializableObject, Has.Property("DeliveryArea").EqualTo(_flexibleEmpl.DeliveryArea));
            Assert.That(serializableObject, Has.Property("TipsEarned").EqualTo(_flexibleEmpl.TipsEarned));
            Assert.That(serializableObject, Has.Property("EmployeeType").EqualTo(_flexibleEmpl.EmployeeType));
        });
    }
}