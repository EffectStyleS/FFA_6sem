namespace DAL.Entities
{
    public class ExpenseType
    {
        public ExpenseType()
        {
            this.Expense = new HashSet<Expense>();
            this.PlannedExpenses = new HashSet<PlannedExpenses>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Expense> Expense { get; set; }
        public virtual ICollection<PlannedExpenses> PlannedExpenses { get; set; }
    }
}
