using CashInn.Enum;
using CashInn.Model.Employee;

namespace Tests.model.Employee;

[TestFixture]
[TestOf(typeof(Waiter))]
public class WaiterTest
{
    private Waiter _waiter = null!;
    
    [SetUp]
    public void SetUp()
    {
        _waiter = new Waiter(
            3,
            "Test Waiter",
            28000,
            DateTime.Now.AddYears(-1),
            DateTime.Today.AddHours(11),
            DateTime.Today.AddHours(19),
            StatusEmpl.FullTime,
            false,
            50.0
        );
    }

    [Test]
    public void TipsEarned_SetNegative_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _waiter.TipsEarned = -1.0);
    }

    [Test]
    public void TipsEarned_SetPositive_ShouldSet()
    {
        _waiter.TipsEarned = 75.0;
        Assert.That(_waiter.TipsEarned, Is.EqualTo(75.0));
    }
    
    [Test]
    public void ToSerializableObject_ShouldReturnExpectedObject()
    {
        var serializableObject = _waiter.ToSerializableObject();
        
        Assert.Multiple(() =>
        {
            Assert.That(serializableObject, Has.Property("Id").EqualTo(_waiter.Id));
            Assert.That(serializableObject, Has.Property("Name").EqualTo(_waiter.Name));
            Assert.That(serializableObject, Has.Property("Salary").EqualTo(_waiter.Salary));
            Assert.That(serializableObject, Has.Property("HireDate").EqualTo(_waiter.HireDate));
            Assert.That(serializableObject, Has.Property("ShiftStart").EqualTo(_waiter.ShiftStart));
            Assert.That(serializableObject, Has.Property("ShiftEnd").EqualTo(_waiter.ShiftEnd));
            Assert.That(serializableObject, Has.Property("Status").EqualTo(_waiter.Status));
            Assert.That(serializableObject, Has.Property("IsBranchManager").EqualTo(_waiter.IsBranchManager));
            Assert.That(serializableObject, Has.Property("LayoffDate").EqualTo(_waiter.LayoffDate));
            Assert.That(serializableObject, Has.Property("TipsEarned").EqualTo(_waiter.TipsEarned));
            Assert.That(serializableObject, Has.Property("EmployeeType").EqualTo(_waiter.EmployeeType));
        });
    }
}