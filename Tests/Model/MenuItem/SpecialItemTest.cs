using CashInn.Model.MenuItem;

namespace Tests.model.MenuItem;

[TestFixture]
[TestOf(typeof(SpecialItem))]
public class SpecialItemTest
{
 private SpecialItem _specialItem = null!;

    [SetUp]
    public void SetUp()
    {
        _specialItem = new SpecialItem(
            1,
            "Test Special Item",
            15.99,
            "A special test item",
            "Contains nuts",
            true,
            DateTime.Now.AddDays(-1),  // ValidFrom in the past
            DateTime.Now.AddDays(1)    // ValidTo in the future
        );
    }

    [Test]
    public void ValidFrom_SetFutureDate_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _specialItem.ValidFrom = DateTime.Now.AddDays(1));
    }

    [Test]
    public void ValidFrom_SetPastDate_ShouldSet()
    {
        var validFromDate = DateTime.Now.AddDays(-5);
        _specialItem.ValidFrom = validFromDate;
        Assert.That(_specialItem.ValidFrom, Is.EqualTo(validFromDate));
    }

    [Test]
    public void ValidTo_SetBeforeValidFrom_ShouldThrowException()
    {
        _specialItem.ValidFrom = DateTime.Now.AddDays(-1);
        Assert.Throws<ArgumentException>(() => _specialItem.ValidTo = DateTime.Now.AddDays(-2));
    }

    [Test]
    public void ValidTo_SetAfterValidFrom_ShouldSet()
    {
        var validToDate = DateTime.Now.AddDays(3);
        _specialItem.ValidTo = validToDate;
        Assert.That(_specialItem.ValidTo, Is.EqualTo(validToDate));
    }

    [Test]
    public void ValidTo_SetSameAsValidFrom_ShouldSet()
    {
        _specialItem.ValidFrom = DateTime.Now.AddDays(-1);
        _specialItem.ValidTo = _specialItem.ValidFrom;
        Assert.That(_specialItem.ValidTo, Is.EqualTo(_specialItem.ValidFrom));
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _specialItem.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _specialItem.Id = 2;
        Assert.That(_specialItem.Id, Is.EqualTo(2));
    }

    [Test]
    public void Name_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _specialItem.Name = null);
    }

    [Test]
    public void Name_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _specialItem.Name = "  ");
    }

    [Test]
    public void Name_SetNonWhiteSpace_ShouldSet()
    {
        _specialItem.Name = "Valid Special Item Name";
        Assert.That(_specialItem.Name, Is.EqualTo("Valid Special Item Name"));
    }

    [Test]
    public void Price_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _specialItem.Price = -1);
    }

    [Test]
    public void Price_SetPositiveValue_ShouldSet()
    {
        _specialItem.Price = 20.00;
        Assert.That(_specialItem.Price, Is.EqualTo(20.00));
    }

    [Test]
    public void Description_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _specialItem.Description = null);
    }

    [Test]
    public void Description_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _specialItem.Description = "  ");
    }

    [Test]
    public void Description_SetNonWhiteSpace_ShouldSet()
    {
        _specialItem.Description = "Valid description";
        Assert.That(_specialItem.Description, Is.EqualTo("Valid description"));
    }

    [Test]
    public void DietaryInformation_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _specialItem.DietaryInformation = null);
    }

    [Test]
    public void DietaryInformation_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _specialItem.DietaryInformation = "  ");
    }

    [Test]
    public void DietaryInformation_SetNonWhiteSpace_ShouldSet()
    {
        _specialItem.DietaryInformation = "Vegetarian";
        Assert.That(_specialItem.DietaryInformation, Is.EqualTo("Vegetarian"));
    }

    [Test]
    public void Available_SetToTrue_ShouldSet()
    {
        _specialItem.Available = true;
        Assert.That(_specialItem.Available, Is.True);
    }
    
    [Test]
    public void ItemType_ShouldReturnSpecial()
    {
        Assert.That(_specialItem.ItemType, Is.EqualTo("Special"));
    }
    
    [Test]
    public void ToSerializableObject_ShouldReturnExpectedObject()
    {
        var serializableObject = _specialItem.ToSerializableObject();
        
        Assert.Multiple(() =>
        {
            Assert.That(serializableObject, Has.Property("Id").EqualTo(_specialItem.Id));
            Assert.That(serializableObject, Has.Property("Name").EqualTo(_specialItem.Name));
            Assert.That(serializableObject, Has.Property("Price").EqualTo(_specialItem.Price));
            Assert.That(serializableObject, Has.Property("Description").EqualTo(_specialItem.Description));
            Assert.That(serializableObject, Has.Property("DietaryInformation").EqualTo(_specialItem.DietaryInformation));
            Assert.That(serializableObject, Has.Property("Available").EqualTo(_specialItem.Available));
            Assert.That(serializableObject, Has.Property("ValidFrom").EqualTo(_specialItem.ValidFrom));
            Assert.That(serializableObject, Has.Property("ValidTo").EqualTo(_specialItem.ValidTo));
            Assert.That(serializableObject, Has.Property("ItemType").EqualTo(_specialItem.ItemType));
        });
    }
}