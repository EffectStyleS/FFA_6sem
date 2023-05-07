using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Entities
{
    public partial class FFAContext : IdentityDbContext<User>
    {
        #region Constructor
        //public FFAContext(DbContextOptions<FFAContext>
        //options)
        //: base(options)
        //{ }

        protected readonly IConfiguration Configuration;

        public FFAContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
        #endregion

        public virtual DbSet<Budget> Budget { get; private set; }
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<ExpenseType> ExpenseType { get; set; }
        public virtual DbSet<Income> Income { get; set; }
        public virtual DbSet<IncomeType> IncomeType { get; set; }
        public virtual DbSet<PlannedExpenses> PlannedExpenses { get; set; }
        public virtual DbSet<PlannedIncomes> PlannedIncomes { get; set; }
        public virtual DbSet<TimePeriod> TimePeriod { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.HasOne(b => b.User)
                .WithMany(u => u.Budget)
                .HasForeignKey(b => b.UserId);

                entity.HasOne(b => b.TimePeriod)
                .WithMany(t => t.Budget)
                .HasForeignKey(b => b.TimePeriodId);
            });

            modelBuilder.Entity<Income>(entity =>
            {
                entity.HasOne(i => i.User)
                .WithMany(u => u.Income)
                .HasForeignKey(i => i.UserId);

                entity.HasOne(i => i.IncomeType)
                .WithMany(it => it.Income)
                .HasForeignKey(i => i.IncomeTypeId);
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasOne(e => e.User)
                .WithMany(u => u.Expense)
                .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.ExpenseType)
                .WithMany(et => et.Expense)
                .HasForeignKey(e => e.ExpenseTypeId);
            });

            modelBuilder.Entity<PlannedExpenses>(entity =>
            {
                entity.HasOne(pe => pe.Budget)
                .WithMany(b => b.PlannedExpenses)
                .HasForeignKey(pe => pe.BudgetId);

                entity.HasOne(pe => pe.ExpenseType)
                .WithMany(et => et.PlannedExpenses)
                .HasForeignKey(pe => pe.ExpenseTypeId);
            });

            modelBuilder.Entity<PlannedIncomes>(entity =>
            {
                entity.HasOne(pi => pi.Budget)
                .WithMany(b => b.PlannedIncomes)
                .HasForeignKey(pi => pi.BudgetId);

                entity.HasOne(pi => pi.IncomeType)
                .WithMany(it => it.PlannedIncomes)
                .HasForeignKey(pi => pi.IncomeTypeId);
            });
        }

    }
}
