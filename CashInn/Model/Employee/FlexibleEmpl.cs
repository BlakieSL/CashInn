using CashInn.Enum;
using CashInn.Model.FlexibleEmplSetup;

namespace CashInn.Model.Employee
{
    public class FlexibleEmpl : DeliveryEmpl, IWaiterEmpl
    {
        private readonly List<Table> _assignedTables = new();
        public IEnumerable<Table> AssignedTables => _assignedTables.AsReadOnly();

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

        public override string EmployeeType => "FlexibleEmpl";

        public FlexibleEmpl(int id, string name, double salary, DateTime hireDate, DateTime shiftStart, DateTime shiftEnd,
            StatusEmpl status, bool isBranchManager, string vehicle, string deliveryArea, double tipsEarned, Branch employerBranch,
            Branch? managedBranch = null, DateTime? layoffDate = null)
            : base(id, name, salary, hireDate, shiftStart, shiftEnd, status, isBranchManager, vehicle, deliveryArea, employerBranch, managedBranch, layoffDate)
        {
            TipsEarned = tipsEarned;
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
                Vehicle,
                DeliveryArea,
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
