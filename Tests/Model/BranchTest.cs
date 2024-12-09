using CashInn.Model;
using CashInn.Model.Employee;
using CashInn.Enum;

namespace Tests.Model;

[TestFixture]
[TestOf(typeof(Branch))]
public class BranchTest
{
    private Branch _branch = null!;
    private Waiter _waiter = null!;
    private Waiter _manager = null!;
    private const string TestFilePath = "Branches.json";

    [SetUp]
    public void SetUp()
    {
        // Initialize a Branch
        _branch = new Branch(1, "Main Street", "123-456-7890");

        // Initialize a Waiter as a manager
        _manager = new Waiter(
            id: 1,
            name: "Alice Johnson",
            salary: 3500.0,
            hireDate: DateTime.Now.AddYears(-2),
            shiftStart: DateTime.Today.AddHours(9),
            shiftEnd: DateTime.Today.AddHours(17),
            status: StatusEmpl.FullTime,
            isBranchManager: true,
            tipsEarned: 0,
            employerBranch: _branch
        );

        // Initialize a Waiter as a regular employee
        _waiter = new Waiter(
            id: 2,
            name: "Bob Smith",
            salary: 2700.0,
            hireDate: DateTime.Now.AddYears(-1),
            shiftStart: DateTime.Today.AddHours(10),
            shiftEnd: DateTime.Today.AddHours(18),
            status: StatusEmpl.FullTime,
            isBranchManager: false,
            tipsEarned: 500.0,
            employerBranch: _branch
        );

        // Clean up the test file if it exists
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

        // Clear and load extent
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
    public void AddEmployee_NullEmployee_ShouldThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => _branch.AddEmployee(null!));
    }

    //
    // [Test]
    // public void AddEmployee_ValidWaiter_ShouldAddWaiter()
    // {
    //     var newWaiter = new Waiter(
    //         id: 4,
    //         name: "John Doe",
    //         salary: 2500.0,
    //         hireDate: DateTime.Now.AddMonths(-6),
    //         shiftStart: DateTime.Today.AddHours(11),
    //         shiftEnd: DateTime.Today.AddHours(19),
    //         status: StatusEmpl.FullTime,
    //         isBranchManager: false,
    //         tipsEarned: 300.0,
    //         employerBranch: _branch
    //     );
    //
    //      //the problem with the method AddEmployee
    //     _branch.AddEmployee(newWaiter);
    //     Assert.That(_branch.Employees, Contains.Item(newWaiter));
    // }

    [Test]
    public void RemoveEmployee_ValidWaiter_ShouldRemoveWaiter()
    {
        _branch.RemoveEmployee(_waiter);
        Assert.That(_branch.Employees, Does.Not.Contain(_waiter));
    }

    [Test]
    public void AddManager_NullManager_ShouldThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => _branch.AddManager(null!));
    }

    [Test]
    public void AddManager_ValidWaiter_ShouldSetManager()
    {
        _branch.AddManager(_manager);
        Assert.That(_branch.Manager, Is.EqualTo(_manager));
    }

    [Test]
    public void RemoveManager_NoManager_ShouldNotThrow()
    {
        Assert.DoesNotThrow(() => _branch.RemoveManager());
        Assert.That(_branch.Manager, Is.Null);
    }

    [Test]
    public void RemoveManager_ManagerAssigned_ShouldRemoveManager()
    {
        _branch.AddManager(_manager);
        _branch.RemoveManager();
        Assert.That(_branch.Manager, Is.Null);
    }
//TODO fix this test so it will be done correctly
    // [Test]
    // public void Encapsulation_ShouldNotAllowDirectModification()
    // {
    //     _branch.Location = "Modified Location";
    //     Assert.AreEqual("Modified Location", _branch.Location);
    //
    //     Assert.IsTrue(Branch.GetAll().Any(b => b.Location == "Modified Location"));
    // }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredBranchesCorrectly()
    {
        Branch.ClearExtent();
        var branch1 = new Branch(1, "Main Street", "555-0123");
        var branch2 = new Branch(2, "Second Avenue", "555-0456");

        Branch.SaveExtent();
        Branch.LoadExtent();

        var loadedBranches = Branch.GetAll();
        Assert.AreEqual(2, loadedBranches.Count);
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
