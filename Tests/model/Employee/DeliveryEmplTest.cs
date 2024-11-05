using CashInn.Enum;
using CashInn.Model.Employee;

namespace Tests.model.Employee;

[TestFixture]
[TestOf(typeof(DeliveryEmpl))]
public class DeliveryEmplTest
{
    private DeliveryEmpl _deliveryEmpl = null!;
    
    [SetUp]
    public void SetUp()
    {
        _deliveryEmpl = new DeliveryEmpl(
            1,
            "Test Delivery Employee",
            32000,
            DateTime.Now.AddYears(-1),
            DateTime.Today.AddHours(9),
            DateTime.Today.AddHours(17),
            StatusEmpl.FullTime,
            false,
            "Scooter",
            "Downtown Area"
        );
    }

    [Test]
    public void Vehicle_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _deliveryEmpl.Vehicle = null);
    }
    
    [Test]
    public void Vehicle_Empty_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _deliveryEmpl.Vehicle = "   ");
    }

    [Test]
    public void Vehicle_SetNonNull_ShouldSet()
    {
        _deliveryEmpl.Vehicle = "Bike";
        Assert.That(_deliveryEmpl.Vehicle, Is.EqualTo("Bike"));
    }

    [Test]
    public void DeliveryArea_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _deliveryEmpl.DeliveryArea = null);
    }
    
    [Test]
    public void DeliveryArea_Empty_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _deliveryEmpl.DeliveryArea = "   ");
    }

    [Test]
    public void DeliveryArea_SetNonNull_ShouldSet()
    {
        _deliveryEmpl.DeliveryArea = "Uptown";
        Assert.That(_deliveryEmpl.DeliveryArea, Is.EqualTo("Uptown"));
    }
    
    [Test]
    public void ToSerializableObject_ShouldReturnExpectedObject()
    {
        var serializableObject = _deliveryEmpl.ToSerializableObject();
        
        Assert.Multiple(() =>
        {
            Assert.That(serializableObject, Has.Property("Id").EqualTo(_deliveryEmpl.Id));
            Assert.That(serializableObject, Has.Property("Name").EqualTo(_deliveryEmpl.Name));
            Assert.That(serializableObject, Has.Property("Salary").EqualTo(_deliveryEmpl.Salary));
            Assert.That(serializableObject, Has.Property("HireDate").EqualTo(_deliveryEmpl.HireDate));
            Assert.That(serializableObject, Has.Property("ShiftStart").EqualTo(_deliveryEmpl.ShiftStart));
            Assert.That(serializableObject, Has.Property("ShiftEnd").EqualTo(_deliveryEmpl.ShiftEnd));
            Assert.That(serializableObject, Has.Property("Status").EqualTo(_deliveryEmpl.Status));
            Assert.That(serializableObject, Has.Property("IsBranchManager").EqualTo(_deliveryEmpl.IsBranchManager));
            Assert.That(serializableObject, Has.Property("LayoffDate").EqualTo(_deliveryEmpl.LayoffDate));
            Assert.That(serializableObject, Has.Property("Vehicle").EqualTo(_deliveryEmpl.Vehicle));
            Assert.That(serializableObject, Has.Property("DeliveryArea").EqualTo(_deliveryEmpl.DeliveryArea));
            Assert.That(serializableObject, Has.Property("EmployeeType").EqualTo(_deliveryEmpl.EmployeeType));
        });
    }
}