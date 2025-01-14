namespace CashInn.Model.FlexibleEmplSetup
{
    public interface IWaiterEmpl
    {
        double TipsEarned { get; set; }
        IEnumerable<Table> AssignedTables { get; }

        void AddTable(Table table)
        {
            ArgumentNullException.ThrowIfNull(table);

            if (table.Waiter != null && table.Waiter != this)
            {
                throw new InvalidOperationException("Table is already assigned to another Waiter-like employee.");
            }

            AddTableInternal(table);
            table.AddWaiter(this);
        }

        void RemoveTable(Table table)
        {
            ArgumentNullException.ThrowIfNull(table);

            if (!AssignedTables.Contains(table)) return;

            RemoveTableInternal(table);
            table.RemoveWaiter();
        }

        void UpdateTable(Table oldTable, Table newTable)
        {
            ArgumentNullException.ThrowIfNull(oldTable);
            ArgumentNullException.ThrowIfNull(newTable);

            if (!AssignedTables.Contains(oldTable))
            {
                throw new InvalidOperationException($"{this.GetType().Name} does not manage oldTable.");
            }

            RemoveTable(oldTable);
            AddTable(newTable);
        }

        void AddTableInternal(Table table);

        void RemoveTableInternal(Table table);
    }
}