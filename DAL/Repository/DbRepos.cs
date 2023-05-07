using DAL.Entities;
using DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class DbRepos : IDbRepos
    {
        private FFAContext _db;
        private UserRepository _userRepository;
        private BudgetRepository _budgetRepository;
        private ExpenseRepository _expenseRepository;
        private ExpenseTypeRepository _expenseTypesRepository;
        private IncomeRepository _incomeRepository;
        private IncomeTypeRepository _incomeTypesRepository;
        private PlannedExpensesRepository _plannedExpensesRepository;
        private PlannedIncomesRepository _plannedIncomesRepository;
        private TimePeriodRepository _timePeriodRepository;

        public DbRepos(IConfiguration configuration)
        {
            _db = new FFAContext(configuration);
        }

        public IUserRepository<User> User
        {
            get
            {
                _userRepository ??= new UserRepository(_db);
                return _userRepository;
            }
        }


        public IRepository<Budget> Budget
        {
            get
            {
                _budgetRepository ??= new BudgetRepository(_db);
                return _budgetRepository;
            }
        }


        public IRepository<PlannedExpenses> PlannedExpenses
        {
            get
            {
                _plannedExpensesRepository ??= new PlannedExpensesRepository(_db);
                return _plannedExpensesRepository;
            }
        }

        public IRepository<PlannedIncomes> PlannedIncomes
        {
            get
            {
                _plannedIncomesRepository ??= new PlannedIncomesRepository(_db);
                return _plannedIncomesRepository;
            }
        }

        public IRepository<Expense> Expense
        {
            get
            {
                _expenseRepository ??= new ExpenseRepository(_db);
                return _expenseRepository;
            }
        }

        public IRepository<Income> Income
        {
            get
            {
                _incomeRepository ??= new IncomeRepository(_db);
                return _incomeRepository;
            }
        }

        public IRepository<ExpenseType> ExpenseType
        {
            get
            {
                _expenseTypesRepository ??= new ExpenseTypeRepository(_db);
                return _expenseTypesRepository;
            }
        }

        public IRepository<IncomeType> IncomeType
        {
            get
            {
                _incomeTypesRepository ??= new IncomeTypeRepository(_db);
                return _incomeTypesRepository;
            }
        }

        public IRepository<TimePeriod> TimePeriod
        {
            get
            {
                _timePeriodRepository ??= new TimePeriodRepository(_db);
                return _timePeriodRepository;
            }
        }

        public async Task<int> Save()
        {
            try
            {
                return await _db.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
