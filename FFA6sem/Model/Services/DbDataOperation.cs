using DAL.Entities;
using DAL.Interfaces;
using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Models;
using Microsoft.AspNetCore.Identity;

namespace FFA6sem.Model.Services
{
    public class DbDataOperation : IDbCrud
    {
        IDbRepos _db;
        public DbDataOperation(IDbRepos db)
        {
            _db = db;
        }

        #region Create

        public async Task CreateBudget(BudgetModel budget)
        {
            _db.Budget.Create(
                new Budget()
                {
                    Id           = budget.Id,
                    StartDate    = budget.StartDate,
                    TimePeriodId = budget.TimePeriodId,
                    UserId       = budget.UserId,
                });

            await Save();
        }

        public async Task CreateExpense(ExpenseModel expense)
        {
            _db.Expense.Create(
                new Expense()
                {
                    Id            = expense.Id,
                    Name          = expense.Name,
                    Value         = expense.Value,
                    Date          = expense.Date,
                    ExpenseTypeId = expense.ExpenseTypeId,
                    UserId        = expense.UserId
                });
            await Save();
        }

        public async Task CreateExpenseType(ExpenseTypeModel expenseType)
        {
            _db.ExpenseType.Create(
                new ExpenseType()
                {
                    Id   = expenseType.Id,
                    Name = expenseType.Name
                });
            await Save();
        }

        public async Task CreateIncome(IncomeModel income)
        {
            _db.Income.Create(
                new Income()
                {
                    Id           = income.Id,
                    Name         = income.Name,
                    Value        = income.Value,
                    Date         = income.Date,
                    IncomeTypeId = income.IncomeTypeId,
                    UserId       = income.UserId
                });
            await Save();
        }

        public async Task CreateIncomeType(IncomeTypeModel incomeType)
        {
            _db.IncomeType.Create(
                new IncomeType()
                {
                    Id   = incomeType.Id,
                    Name = incomeType.Name
                });
            await Save();
        }

        public async Task CreatePlannedExpenses(PlannedExpensesModel plannedExpenses)
        {
            _db.PlannedExpenses.Create(
                new PlannedExpenses()
                {
                    Id            = plannedExpenses.Id,
                    BudgetId      = plannedExpenses.BudgetId,
                    Sum           = plannedExpenses.Sum,
                    ExpenseTypeId = plannedExpenses.ExpenseTypeId,
                });
            await Save();
        }

        public async Task CreatePlannedIncomes(PlannedIncomesModel plannedIncomes)
        {
            _db.PlannedIncomes.Create(
                new PlannedIncomes()
                {
                    Id           = plannedIncomes.Id,
                    BudgetId     = plannedIncomes.BudgetId,
                    Sum          = plannedIncomes.Sum,
                    IncomeTypeId = plannedIncomes.IncomeTypeId,
                });
            await Save();
        }

        public async Task CreateTimePeriod(TimePeriodModel timePeriod)
        {
            _db.TimePeriod.Create(
                new TimePeriod()
                {
                    Id   = timePeriod.Id,
                    Name = timePeriod.Name
                });
            await Save();
        }

        public async Task<IdentityResult> CreateUser(UserModel userModel, UserManager<User> userManager)
        {
            User newUser = new User()
            {
                UserName = userModel.UserName,
            };

            var result = await _db.User.Create(newUser, userModel.Password, userManager);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, userModel.Role);
                await Save();
            }

            return result;

        }
        #endregion

        #region Delete
        public async Task DeleteBudget(int id)
        {
            if (await _db.Budget.GetItem(id) != null)
            {
                _db.Budget.Delete(id);
                await Save();
            }
        }

        public async Task DeleteExpense(int id)
        {
            if (await _db.Expense.GetItem(id) != null)
            {
                _db.Expense.Delete(id);
                await Save();
            }
        }

        public async Task DeleteExpenseType(int id)
        {
            if (_db.ExpenseType.GetItem(id) != null)
            {
                _db.ExpenseType.Delete(id);
                await Save();
            }
        }

        public async Task DeleteIncome(int id)
        {
            if (_db.Income.GetItem(id) != null)
            {
                _db.Income.Delete(id);
                await Save();
            }
        }

        public async Task DeleteIncomeType(int id)
        {
            if (_db.IncomeType.GetItem(id) != null)
            {
                _db.IncomeType.Delete(id);
                await Save();
            }
        }

        public async Task DeletePlannedExpenses(int id)
        {
            if (_db.PlannedExpenses.GetItem(id) != null)
            {
                _db.PlannedExpenses.Delete(id);
                await Save();
            }
        }

        public async Task DeletePlannedIncomes(int id)
        {
            if (_db.PlannedIncomes.GetItem(id) != null)
            {
                _db.PlannedIncomes.Delete(id);
                await Save();
            }
        }

        public async Task DeleteTimePeriod(int id)
        {
            if (_db.TimePeriod.GetItem(id) != null)
            {
                _db.TimePeriod.Delete(id);
                await Save();
            }
        }

        public async Task DeleteUser(string id, UserManager<User> userManager)
        {
            if (await _db.User.GetItem(id, userManager) != null)
            {
                await _db.User.Delete(id, userManager);
                await Save();
            }
        }
        #endregion

        #region GetAll
        public async Task<List<PlannedExpensesModel>> GetAllBudgetsPlannedExpenses(int budgetId)
        {
            var budgetPlannedExpenses = await _db.PlannedExpenses.GetAll();

            var result = budgetPlannedExpenses
                .Where(e => e.BudgetId == budgetId)
                .Select(e => new PlannedExpensesModel(e))
                .ToList();

            return result;
        }

        public async Task<List<PlannedIncomesModel>> GetAllBudgetsPlannedIncomes(int budgetId)
        {
            var budgetPlannedIncomes = await _db.PlannedIncomes.GetAll();

            var result = budgetPlannedIncomes
                .Where(e => e.BudgetId == budgetId)
                .Select(e => new PlannedIncomesModel(e))
                .ToList();

            return result;
        }

        public async Task<List<ExpenseModel>> GetAllExpenses(string userId)
        {
            var expenses = await _db.Expense.GetAll();

            var result = expenses
                .Where(e => e.UserId == userId)
                .Select(e => new ExpenseModel(e))
                .ToList();

            return result;
        }

        public async Task<List<ExpenseTypeModel>> GetAllExpenseTypes()
        {
            var expenseTypes = await _db.ExpenseType.GetAll();

            var result = expenseTypes
                .Select(e => new ExpenseTypeModel(e))
                .ToList();

            return result;
        }

        public async Task<List<IncomeModel>> GetAllIncomes(string userId)
        {
            var incomes = await _db.Income.GetAll();

            var result = incomes
                .Where(e => e.UserId == userId)
                .Select(e => new IncomeModel(e))
                .ToList();

            return result;
        }

        public async Task<List<IncomeTypeModel>> GetAllIncomeTypes()
        {
            var incomeTypes = await _db.IncomeType.GetAll();

            var result = incomeTypes
                .Select(e => new IncomeTypeModel(e))
                .ToList();

            return result;
        }

        public async Task<List<TimePeriodModel>> GetAllTimePeriods()
        {
            var timePeriods = await _db.TimePeriod.GetAll();

            var result = timePeriods
                .Select(e => new TimePeriodModel(e))
                .ToList();
            return result;
        }

        public async Task<List<BudgetModel>> GetAllUserBudgets(string userId)
        {
            var budgets = await _db.Budget.GetAll();

            var result = budgets
                .Where(e => e.UserId == userId)
                .Select(e => new BudgetModel(e))
                .ToList();
       
            return result;
        }

        public async Task<List<BudgetModel>> GetAllBudgets()
        {
            var budgets = await _db.Budget.GetAll();

            var result = budgets
                .Select(e => new BudgetModel(e))
                .ToList();

            return result;
        }

        public async Task<List<UserModel>> GetAllUsers(UserManager<User> userManager)
        {
            var users = await _db.User.GetAll();

            var result = users
                .Select(e => new UserModel(e, userManager))
                .ToList();

            return result;
        }
        #endregion

        #region GetById
        public async Task<PlannedExpensesModel> GetBudgetsPlannedExpensesById(int id)
        {
            var plannedExpenses = await _db.PlannedExpenses.GetItem(id);

            if (plannedExpenses == null)
            {
                return null;
            }

            return new PlannedExpensesModel(plannedExpenses);
        }

        public async Task<PlannedIncomesModel> GetBudgetsPlannedIncomesById(int id)
        {
            var plannedIncomes = await _db.PlannedIncomes.GetItem(id);

            if (plannedIncomes == null)
            {
                return null;
            }

            return new PlannedIncomesModel(plannedIncomes);
        }

        public async Task<ExpenseModel> GetExpenseById(int id)
        {
            var expense = await _db.Expense.GetItem(id);

            if (expense == null)
            {
                return null;
            }    

            return new ExpenseModel(expense);
        }

        public async Task<ExpenseTypeModel> GetExpenseTypeById(int id)
        {
            var expenseType = await _db.ExpenseType.GetItem(id);

            if (expenseType == null)
            {
                return null;
            }
            return new ExpenseTypeModel(expenseType);
        }

        public async Task<IncomeModel> GetIncomeById(int id)
        {
            var income = await _db.Income.GetItem(id);

            if (income == null)
            {
                return null;
            }

            return new IncomeModel(income);
        }

        public async Task<IncomeTypeModel> GetIncomeTypeById(int id)
        {
            var incomeType = await _db.IncomeType.GetItem(id);

            if (incomeType == null)
            {
                return null;
            }

            return new IncomeTypeModel(incomeType);
        }

        public async Task<TimePeriodModel> GetTimePeriodsById(int id)
        {
            var timePeriod = await _db.TimePeriod.GetItem(id);

            if (timePeriod == null)
            {
                return null;
            }

            return new TimePeriodModel(timePeriod);
        }

        public async Task<BudgetModel> GetUserBudgetById(int id)
        {
            var budget = await _db.Budget.GetItem(id);

            if (budget == null)
            {
                return null;
            }

            return new BudgetModel(budget);
        }

        public async Task<UserModel> GetUserById(string id, UserManager<User> userManager)
        {
            var user = await _db.User.GetItem(id, userManager);

            if (user == null)
            {
                return null;
            }

            return new UserModel(user, userManager);
        }
        #endregion

        #region Update

        public async Task UpdateBudget(BudgetModel budget)
        {
            try
            {
                Budget b       = await  _db.Budget.GetItem(budget.Id);
                b.Id           = budget.Id;
                b.StartDate    = budget.StartDate;
                b.TimePeriodId = budget.TimePeriodId;
                b.UserId       = budget.UserId;

                _db.Budget.Update(b);
                await Save();
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateExpense(ExpenseModel expense)
        {
            try
            {
                Expense e       = await _db.Expense.GetItem(expense.Id);
                e.Id            = expense.Id;
                e.Date          = expense.Date;
                e.Name          = expense.Name;
                e.Value         = expense.Value;
                e.ExpenseTypeId = expense.ExpenseTypeId;
                e.UserId        = expense.UserId;

                _db.Expense.Update(e);
                await Save();
            }
            catch
            {
                throw;
            }


        }

        public async Task UpdateExpenseType(ExpenseTypeModel expenseType)
        {
            try
            {
                ExpenseType et = await _db.ExpenseType.GetItem(expenseType.Id);
                et.Id          = expenseType.Id;
                et.Name        = expenseType.Name;

                _db.ExpenseType.Update(et);
                await Save();
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateIncome(IncomeModel income)
        {
            try
            {
                Income i       = await _db.Income.GetItem(income.Id);
                i.Id           = income.Id;
                i.Date         = income.Date;
                i.Name         = income.Name;
                i.Value        = income.Value;
                i.IncomeTypeId = income.IncomeTypeId;
                i.UserId       = income.UserId;

                _db.Income.Update(i);
                await Save();
            }
            catch
            {
                throw;
            }

        }

        public async Task UpdateIncomeType(IncomeTypeModel incomeType)
        {
            try
            {
                IncomeType it = await _db.IncomeType.GetItem(incomeType.Id);
                it.Id         = incomeType.Id;
                it.Name       = incomeType.Name;

                _db.IncomeType.Update(it);
                await Save();
            }
            catch
            {
                throw;
            }

        }

        public async Task UpdatePlannedExpenses(PlannedExpensesModel plannedExpenses)
        {
            try
            {
                PlannedExpenses pe = await _db.PlannedExpenses.GetItem(plannedExpenses.Id);
                pe.Id              = plannedExpenses.Id;
                pe.Sum             = plannedExpenses.Sum;
                pe.BudgetId        = plannedExpenses.BudgetId;
                pe.ExpenseTypeId   = plannedExpenses.ExpenseTypeId;

                _db.PlannedExpenses.Update(pe);
                await Save();
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdatePlannedIncomes(PlannedIncomesModel plannedIncomes)
        {
            try 
            {
                PlannedIncomes pi = await _db.PlannedIncomes.GetItem(plannedIncomes.Id);
                pi.Id             = plannedIncomes.Id;
                pi.Sum            = plannedIncomes.Sum;
                pi.BudgetId       = plannedIncomes.BudgetId;
                pi.IncomeTypeId   = plannedIncomes.IncomeTypeId;

                _db.PlannedIncomes.Update(pi);
                await Save();
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateTimePeriod(TimePeriodModel timePeriod)
        {
            try 
            {
                TimePeriod tp = await _db.TimePeriod.GetItem(timePeriod.Id);
                tp.Id         = timePeriod.Id;
                tp.Name       = timePeriod.Name;

                _db.TimePeriod.Update(tp);
                await Save();
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateUser(UserModel user, UserManager<User> userManager)
        {
            User u         = await _db.User.GetItem(user.Id, userManager);
            u.Id           = user.Id;
            u.UserName     = user.UserName;

            await _db.User.Update(u, userManager);

            await Save();
        }

        #endregion

        public async Task<bool> Save()
        {   
            try
            {
                if (await _db.Save() > 0)
                    return true;

                return false;
            }
            catch
            {
                throw;
            }

        }
    }
}
