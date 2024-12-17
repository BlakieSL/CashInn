using CashInn.Model;

namespace CashInn.Tests
{
    public class MenuBranchTests
    {
        Branch _branch = new Branch(1, "loc", "contact");
        
        [Test]
        public void SetBranch_AssignsBranchToMenu_WhenValidBranch()
        {
            var branch = new Branch(1, "Location", "ContactInfo");
            var menu = new Menu(1, DateTime.Now, _branch);

            Assert.That(menu.Branch, Is.EqualTo(branch));
        }

        [Test]
        public void SetBranch_ThrowsInvalidOperationException_WhenMenuAlreadyAssignedToAnotherBranch()
        {
            var branch2 = new Branch(2, "Location 2", "ContactInfo 2");
            var menu = new Menu(1, DateTime.Now, _branch);

            Assert.Throws<InvalidOperationException>(() => menu.SetBranch(branch2));
        }

        [Test]
        public void AddMenu_AssignsMenuToBranch_WhenValidMenu()
        {
            var menu = new Menu(1, DateTime.Now, _branch);

            Assert.That(_branch.Menu, Is.EqualTo(menu));
        }

        [Test]
        public void AddMenu_ThrowsInvalidOperationException_WhenMenuAlreadyAssignedToAnotherBranch()
        {
            var branch2 = new Branch(2, "Location 2", "ContactInfo 2");
            var menu = new Menu(1, DateTime.Now, _branch);

            Assert.Throws<InvalidOperationException>(() => branch2.AddMenu(menu));
        }
        
        [Test]
        public void Branch_AddMenuAndSetBranchProperly()
        {
            var menu = new Menu(101, DateTime.Now, _branch);

            Assert.That(menu.Branch, Is.EqualTo(_branch));
            Assert.That(_branch.Menu, Is.EqualTo(menu));
        }
    }
}