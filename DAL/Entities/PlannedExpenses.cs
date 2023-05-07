namespace DAL.Entities
{
    public class PlannedExpenses
    {
        public int Id { get; set; }
        public Nullable<decimal> Sum { get; set; }
        public int ExpenseTypeId { get; set; }
        public int BudgetId { get; set; }

        public virtual Budget Budget { get; set; }
        public virtual ExpenseType ExpenseType { get; set; }
    }
}
