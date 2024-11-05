using CashInn.Helper;

namespace Tests.Helper;

[TestFixture]
public class ClassExtentTests
{
    private class TestClass : ClassExtent<TestClass>
    {
        protected override string FilePath => "TestClasses.json";

        public int Id { get; set; }

        public TestClass(int id)
        {
            Id = id;
            AddInstance(this);
        }
        
        public TestClass() { }
    }

    [SetUp]
    public void SetUp()
    {
        if (File.Exists("TestClasses.json"))
        {
            File.Delete("TestClasses.json");
        }
        TestClass.ClearExtent();
        TestClass.LoadExtent();
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists("TestClasses.json"))
        {
            File.Delete("TestClasses.json");
        }
        TestClass.ClearExtent();
    }

    [Test]
    public void SaveExtent_ShouldStoreCorrectClasses()
    {
        var instance1 = new TestClass(1);
        var instance2 = new TestClass(2);
        
        TestClass.SaveExtent();
        
        var loadedInstances = TestClass.GetAll();

        Assert.AreEqual(2, loadedInstances.Count);
        Assert.IsTrue(loadedInstances.Any(i => i.Id == 1));
        Assert.IsTrue(loadedInstances.Any(i => i.Id == 2));
    }

    [Test]
    public void Encapsulation_ShouldNotAllowDirectModification()
    {
        var instance = new TestClass(1);
        instance.Id = 5;
        
        Assert.AreEqual(5, instance.Id);
        
        Assert.IsTrue(TestClass.GetAll().Any(i => i.Id == 5));
    }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredInstancesCorrectly()
    {
        var instance1 = new TestClass(1);
        var instance2 = new TestClass(2);
        
        TestClass.SaveExtent(); 
        
        // TestClass.LoadExtent();
        Assert.AreEqual(2, TestClass.GetAll().Count);

        instance1 = null!;
        instance2 = null!;
        TestClass.LoadExtent();

        Assert.AreEqual(2, TestClass.GetAll().Count);
        
        var loadedInstances = TestClass.GetAll();
        Assert.IsTrue(loadedInstances.Any(i => i.Id == 1));
        Assert.IsTrue(loadedInstances.Any(i => i.Id == 2));
    }
}