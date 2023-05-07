namespace DAL.Entities
{
    public class IncomeType
    {
        public IncomeType()
        {
            this.Income = new HashSet<Income>();
            this.PlannedIncomes = new HashSet<PlannedIncomes>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Income> Income { get; set; }
        public virtual ICollection<PlannedIncomes> PlannedIncomes { get; set; }
    }
}
