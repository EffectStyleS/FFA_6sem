namespace DAL.Entities
{
    public class Budget
    {
        public Budget()
        {
            this.PlannedExpenses = new HashSet<PlannedExpenses>();
            this.PlannedIncomes = new HashSet<PlannedIncomes>();
        }

        public int Id { get; set; }
        public System.DateTime StartDate { get; set; }
        public int TimePeriodId { get; set; }
        public string UserId { get; set; }

        public virtual TimePeriod TimePeriod { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<PlannedExpenses> PlannedExpenses { get; set; }
        public virtual ICollection<PlannedIncomes> PlannedIncomes { get; set; }
    }
}
