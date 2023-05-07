using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class User : IdentityUser
    {
        public User()
            : base()
        {
            this.Expense = new HashSet<Expense>();
            this.Income = new HashSet<Income>();
            this.Budget = new HashSet<Budget>();
        }


        public virtual ICollection<Expense> Expense { get; set; }
        public virtual ICollection<Income> Income { get; set; }
        public virtual ICollection<Budget> Budget { get; set; }
    }
}
