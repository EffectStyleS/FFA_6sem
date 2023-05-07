using DAL.Entities;
using FFA6sem.Model.Models;
using Microsoft.AspNetCore.Identity;

namespace FFA6sem.Model.Interfaces
{
    public interface IDbCrud
    {
        Task<List<UserModel>> GetAllUsers(UserManager<User> userManager);
        Task<UserModel> GetUserById(string id, UserManager<User> userManager);
        Task<IdentityResult> CreateUser(UserModel user, UserManager<User> userManager);
        Task UpdateUser(UserModel user, UserManager<User> userManager);
        Task DeleteUser(string id, UserManager<User> userManager);

        Task<List<BudgetModel>> GetAllUserBudgets(string userId);
        Task<List<BudgetModel>> GetAllBudgets();
        Task<BudgetModel> GetUserBudgetById(int id);
        Task CreateBudget(BudgetModel budget);
        Task UpdateBudget(BudgetModel budget);
        Task DeleteBudget(int id);

        Task<List<ExpenseModel>> GetAllExpenses(string userId);
        Task<ExpenseModel> GetExpenseById(int id);
        Task CreateExpense(ExpenseModel expense);
        Task UpdateExpense(ExpenseModel expense);
        Task DeleteExpense(int id);

        Task<List<ExpenseTypeModel>> GetAllExpenseTypes();
        Task<ExpenseTypeModel> GetExpenseTypeById(int id);
        Task CreateExpenseType(ExpenseTypeModel expenseType);
        Task UpdateExpenseType(ExpenseTypeModel expenseType);
        Task DeleteExpenseType(int id);

        Task<List<IncomeModel>> GetAllIncomes(string userId);
        Task<IncomeModel> GetIncomeById(int id);
        Task CreateIncome(IncomeModel income);
        Task UpdateIncome(IncomeModel income);
        Task DeleteIncome(int id);

        Task<List<IncomeTypeModel>> GetAllIncomeTypes();
        Task<IncomeTypeModel> GetIncomeTypeById(int id);
        Task CreateIncomeType(IncomeTypeModel incomeType);
        Task UpdateIncomeType(IncomeTypeModel incomeType);
        Task DeleteIncomeType(int id);

        Task<List<PlannedExpensesModel>> GetAllBudgetsPlannedExpenses(int budgetId);
        Task<PlannedExpensesModel> GetBudgetsPlannedExpensesById(int id);
        Task CreatePlannedExpenses(PlannedExpensesModel plannedExpenses);
        Task UpdatePlannedExpenses(PlannedExpensesModel plannedExpenses);
        Task DeletePlannedExpenses(int id);

        Task<List<PlannedIncomesModel>> GetAllBudgetsPlannedIncomes(int budgetId);
        Task<PlannedIncomesModel> GetBudgetsPlannedIncomesById(int id);
        Task CreatePlannedIncomes(PlannedIncomesModel plannedIncomes);
        Task UpdatePlannedIncomes(PlannedIncomesModel plannedIncomes);
        Task DeletePlannedIncomes(int id);

        Task<List<TimePeriodModel>> GetAllTimePeriods();
        Task<TimePeriodModel> GetTimePeriodsById(int id);
        Task CreateTimePeriod(TimePeriodModel timePeriod);
        Task UpdateTimePeriod(TimePeriodModel timePeriod);
        Task DeleteTimePeriod(int id);
    }
}
