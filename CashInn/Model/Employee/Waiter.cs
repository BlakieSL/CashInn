using CashInn.Enum;
using CashInn.Model.FlexibleEmplSetup;

namespace CashInn.Model.Employee
{
    public class Waiter : AbstractEmployee, IWaiterEmpl
    {
        private readonly List<Table> _assignedTables = new();
        public IEnumerable<Table> AssignedTables => _assignedTables.AsReadOnly();

        public override string EmployeeType => "Waiter";

        private double _tipsEarned;
        public double TipsEarned
        {
            get => _tipsEarned;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Tips earned cannot be negative", nameof(TipsEarned));
                _tipsEarned = value;
            }
        }

        public Waiter(int id, string name, double salary, DateTime hireDate, DateTime shiftStart,
            DateTime shiftEnd, StatusEmpl status, bool isBranchManager, double tipsEarned,
            Branch employerBranch, Branch? managedBranch = null, DateTime? layoffDate = null)
            : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, employerBranch, managedBranch, layoffDate)
        {
            TipsEarned = tipsEarned;
            AddInstance();
        }

        public override object ToSerializableObject()
        {
            return new
            {
                Id,
                Name,
                Salary,
                HireDate,
                ShiftStart,
                ShiftEnd,
                Status,
                IsBranchManager,
                LayoffDate,
                TipsEarned,
                EmployeeType
            };
        }

        public void AddTableInternal(Table table)
        {
            if (!_assignedTables.Contains(table))
            {
                _assignedTables.Add(table);
            }
        }

        public void RemoveTableInternal(Table table)
        {
            _assignedTables.Remove(table);
        }
    }
}
