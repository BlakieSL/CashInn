using CashInn.Model;

namespace Tests.model;

[TestFixture]
[TestOf(typeof(Branch))]
public class BranchTest
{
    private Branch _branch = null!;
    private const string TestFilePath = "Branches.json";

    [SetUp]
    public void SetUp()
    {
        _branch = new Branch(1, "Main Street", "123-456-7890");
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

        Branch.ClearExtent();
        Branch.LoadExtent();
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _branch.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _branch.Id = 2;
        Assert.That(_branch.Id, Is.EqualTo(2));
    }

    [Test]
    public void Location_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _branch.Location = null!);
    }

    [Test]
    public void Location_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _branch.Location = "  ");
    }

    [Test]
    public void Location_SetValidValue_ShouldSet()
    {
        _branch.Location = "New Location";
        Assert.That(_branch.Location, Is.EqualTo("New Location"));
    }

    [Test]
    public void ContactInfo_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _branch.ContactInfo = null!);
    }

    [Test]
    public void ContactInfo_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _branch.ContactInfo = "  ");
    }

    [Test]
    public void ContactInfo_SetValidValue_ShouldSet()
    {
        _branch.ContactInfo = "987-654-3210";
        Assert.That(_branch.ContactInfo, Is.EqualTo("987-654-3210"));
    }
    
    [Test]
    public void Encapsulation_ShouldNotAllowDirectModification()
    {
        var branch = new Branch(1, "Main Street", "555-0123");
        branch.Location = "New Location";

        Assert.AreEqual("New Location", branch.Location);

        Assert.IsTrue(Branch.GetAll().Any(b => b.Location == "New Location"));
    }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredBranchesCorrectly()
    {
        Branch.ClearExtent();
        var branch1 = new Branch(1, "Main Street", "555-0123");
        var branch2 = new Branch(2, "Second Avenue", "555-0456");

        Branch.SaveExtent();

        // Branch.LoadExtent();
        Assert.AreEqual(2, Branch.GetAll().Count);

        branch1 = null!;
        branch2 = null!;
        Branch.LoadExtent();

        Assert.AreEqual(2, Branch.GetAll().Count);

        var loadedBranches = Branch.GetAll();
        Assert.IsTrue(loadedBranches.Any(b => b.Id == 1));
        Assert.IsTrue(loadedBranches.Any(b => b.Id == 2));
    }
    
    [TearDown]
    public void TearDown()
    {
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }
        Branch.ClearExtent();
    }
}