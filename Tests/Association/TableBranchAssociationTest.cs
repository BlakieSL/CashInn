using CashInn.Model;

namespace CashInn.Tests
{
    public class TableBranchAssociationTests
    {
        
        [Test]
        public void SetBranch_AssignsTableToBranch_WhenValidBranch()
        {
            Branch _branch = new Branch(1, "Location", "ContactInfo");
            var table = new Table(1, 4, _branch);

            table.SetBranch(_branch);

            Assert.AreEqual(_branch, table.Branch);
        }

        [Test]
        public void SetBranch_ThrowsInvalidOperationException_WhenTableAlreadyAssignedToAnotherBranch()
        {
            Branch _branch = new Branch(10, "Location", "ContactInfo");

            var branch2 = new Branch(2, "Location 2", "ContactInfo 2");
            var table = new Table(1, 4, _branch);

            Assert.Throws<InvalidOperationException>(() => table.SetBranch(branch2));
        }

        [Test]
        public void RemoveBranch_RemovesTableFromBranch_WhenTableAssignedToBranch()
        {
            Branch _branch = new Branch(10, "Location", "ContactInfo");
            var table = new Table(1, 4, _branch);

            table.RemoveBranch();

            Assert.That(Table.GetAll(), Does.Not.Contain(table));
        }

        [Test]
        public void SetBranch_ThrowsInvalidOperationException_WhenTableNumberAlreadyExistsInBranch()
        {
            Branch _branch = new Branch(1, "Location", "ContactInfo");

            var branch = new Branch(1, "Location", "ContactInfo");
            var table2 = new Table(1, 2, _branch);

            Assert.Throws<InvalidOperationException>(() => table2.SetBranch(branch));
        }

        [Test]
        public void AddTable_AssignsTableToBranch_WhenValidTable()
        {
            Branch _branch = new Branch(1, "Location", "ContactInfo");

            var table = new Table(1, 4, _branch);

            Assert.AreEqual(table, _branch.Tables[table.TableNumber]);
        }

        [Test]
        public void AddTable_ThrowsInvalidOperationException_WhenTableNumberAlreadyExists()
        {
            Branch _branch = new Branch(10, "Location", "ContactInfo");

            var branch = new Branch(1, "Location", "ContactInfo");
            var table2 = new Table(1, 2, _branch);

            Assert.Throws<InvalidOperationException>(() => branch.AddTable(table2));
        }

        [Test]
        public void RemoveTable_RemovesTableFromBranch_WhenTableAssignedToBranch()
        {
            Branch _branch = new Branch(10, "Location", "ContactInfo");
            var table = new Table(1, 4, _branch);

            _branch.RemoveTable(table);

            Assert.IsFalse(_branch.Tables.ContainsKey(table.TableNumber));
        }

        [Test]
        public void AddTable_ThrowsArgumentNullException_WhenTableIsNull()
        {
            var branch = new Branch(1, "Location", "ContactInfo");

            Assert.Throws<ArgumentNullException>(() => branch.AddTable(null!));
        }

        [Test]
        public void RemoveTable_ThrowsArgumentNullException_WhenTableIsNull()
        {
            var branch = new Branch(1, "Location", "ContactInfo");

            Assert.Throws<ArgumentNullException>(() => branch.RemoveTable(null!));
        }

        [Test]
        public void RemoveTable_ThrowsInvalidOperationException_WhenTableNotInBranch()
        {
            Branch _branch = new Branch(10, "Location", "ContactInfo");

            var branch = new Branch(1, "Location", "ContactInfo");
            var table = new Table(1, 4, _branch);

            Assert.Throws<InvalidOperationException>(() => branch.RemoveTable(table));
        }
    }
}