using CashInn.Enum;
using CashInn.Model.MenuItem;

namespace Tests.model.MenuItem;

[TestFixture]
[TestOf(typeof(DefaultItem))]
public class DefaultItemTest
{
private DefaultItem _defaultItem = null!;

    [SetUp]
    public void SetUp()
    {
        _defaultItem = new DefaultItem(
            1,
            "Test Item",
            12.99,
            "A delicious test item",
            "Contains gluten",
            true,
            ServingSize.Medium
        );
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _defaultItem.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _defaultItem.Id = 2;
        Assert.That(_defaultItem.Id, Is.EqualTo(2));
    }

    [Test]
    public void Name_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _defaultItem.Name = null);
    }

    [Test]
    public void Name_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _defaultItem.Name = "  ");
    }

    [Test]
    public void Name_SetNonWhiteSpace_ShouldSet()
    {
        _defaultItem.Name = "Valid Item Name";
        Assert.That(_defaultItem.Name, Is.EqualTo("Valid Item Name"));
    }

    [Test]
    public void Price_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _defaultItem.Price = -1);
    }

    [Test]
    public void Price_SetPositiveValue_ShouldSet()
    {
        _defaultItem.Price = 15.50;
        Assert.That(_defaultItem.Price, Is.EqualTo(15.50));
    }

    [Test]
    public void Description_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _defaultItem.Description = null);
    }

    [Test]
    public void Description_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _defaultItem.Description = "  ");
    }

    [Test]
    public void Description_SetNonWhiteSpace_ShouldSet()
    {
        _defaultItem.Description = "Valid description";
        Assert.That(_defaultItem.Description, Is.EqualTo("Valid description"));
    }

    [Test]
    public void DietaryInformation_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _defaultItem.DietaryInformation = null);
    }

    [Test]
    public void DietaryInformation_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _defaultItem.DietaryInformation = "  ");
    }

    [Test]
    public void DietaryInformation_SetNonWhiteSpace_ShouldSet()
    {
        _defaultItem.DietaryInformation = "Vegan";
        Assert.That(_defaultItem.DietaryInformation, Is.EqualTo("Vegan"));
    }

    [Test]
    public void Available_SetToTrue_ShouldSet()
    {
        _defaultItem.Available = true;
        Assert.That(_defaultItem.Available, Is.True);
    }

    [Test]
    public void ServingSize_SetValidValue_ShouldSet()
    {
        _defaultItem.ServingSize = ServingSize.Big;
        Assert.That(_defaultItem.ServingSize, Is.EqualTo(ServingSize.Big));
    }
    
    [Test]
    public void ToSerializableObject_ShouldReturnExpectedObject()
    {
        var serializableObject = _defaultItem.ToSerializableObject();
        
        Assert.Multiple(() =>
        {
            Assert.That(serializableObject, Has.Property("Id").EqualTo(_defaultItem.Id));
            Assert.That(serializableObject, Has.Property("Name").EqualTo(_defaultItem.Name));
            Assert.That(serializableObject, Has.Property("Price").EqualTo(_defaultItem.Price));
            Assert.That(serializableObject, Has.Property("Description").EqualTo(_defaultItem.Description));
            Assert.That(serializableObject, Has.Property("DietaryInformation").EqualTo(_defaultItem.DietaryInformation));
            Assert.That(serializableObject, Has.Property("Available").EqualTo(_defaultItem.Available));
            Assert.That(serializableObject, Has.Property("ServingSize").EqualTo(_defaultItem.ServingSize));
            Assert.That(serializableObject, Has.Property("ItemType").EqualTo(_defaultItem.ItemType));
        });
    }
}