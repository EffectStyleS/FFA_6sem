using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDbRepos
    {
        IUserRepository<User> User { get; }
        IRepository<Budget> Budget { get; }
        IRepository<PlannedExpenses> PlannedExpenses { get; }
        IRepository<PlannedIncomes> PlannedIncomes { get; }
        IRepository<Expense> Expense { get; }
        IRepository<Income> Income { get; }
        IRepository<ExpenseType> ExpenseType { get; }
        IRepository<IncomeType> IncomeType { get; }
        IRepository<TimePeriod> TimePeriod { get; }

        Task<int> Save();
    }
}
