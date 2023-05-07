namespace DAL.Entities
{
    public class TimePeriod
    {
        public TimePeriod()
        {
            this.Budget = new HashSet<Budget>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Budget> Budget { get; set; }
    }
}
