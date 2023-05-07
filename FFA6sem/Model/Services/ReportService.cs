using DAL.Interfaces;
using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Models;

namespace FFA6sem.Model.Services
{
    public class ReportService : IReportService
    {
        IDbRepos _db;

        public ReportService(IDbRepos db)
        {
            _db = db;
        }

        private DateTime GetEndDate(BudgetModel budget)
        {
            DateTime endDate = new DateTime();
            switch (budget.TimePeriodId)
            {
                case 1:
                    endDate = budget.StartDate.AddMonths(1);
                    break;
                case 2:
                    endDate = budget.StartDate.AddMonths(3);
                    break;
                case 3:
                    endDate = budget.StartDate.AddYears(1);
                    break;
            }

            return endDate;
        }

        public async Task<List<List<decimal?>>> TakeDifferenceOfExpensesSum(UserModel user)
        {
            List<List<decimal?>> differences = new List<List<decimal?>>();

            var helpExpenseTypes = await _db.ExpenseType.GetAll();
            List<ExpenseTypeModel> expenseTypes = helpExpenseTypes.Select(e => new ExpenseTypeModel(e)).ToList();
            List<List<ExpenseModel>> expensesByType = new List<List<ExpenseModel>>();
            List<decimal?> AllSumOfExpensesValues = new List<decimal?>();

            for (int i = 0; i < expenseTypes.Count; i++)
            {
                var helpExpenseByType = await _db.Expense
                    .GetAll();
                var expenseByType = helpExpenseByType
                    .Where(e => e.UserId == user.Id)
                    .Select(e => new ExpenseModel(e))
                    .ToList()
                    .Where(x => x.ExpenseTypeId == expenseTypes[i].Id)
                    .ToList();

                expensesByType.Add(expenseByType);
            }

            var helpUserBudgets = await _db.Budget.GetAll();
            List <BudgetModel> userBudgets = helpUserBudgets.Where(e => e.UserId == user.Id).Select(e => new BudgetModel(e)).ToList();
            List<List<decimal?>> allBudgetsPlannedExpenses = new List<List<decimal?>>();

            foreach (var userBudget in userBudgets)
            {
                var helpPlannedExpenses = await _db.PlannedExpenses
                    .GetAll();
                List<PlannedExpensesModel> plannedExpenses = helpPlannedExpenses
                    .Where(e => e.BudgetId == userBudget.Id)
                    .Select(e => new PlannedExpensesModel(e))
                    .ToList();

                if (plannedExpenses == null)
                {
                    return null;
                }

                List<decimal?> sumsOfPlannedExpenses = new List<decimal?>();
                decimal? sum = 0.0m;

                for (int i = 0; i < expenseTypes.Count; i++)
                {
                    sum = plannedExpenses.Where(x => x.ExpenseTypeId == expenseTypes[i].Id).FirstOrDefault().Sum;
                    sumsOfPlannedExpenses.Add(sum);
                }

                allBudgetsPlannedExpenses.Add(sumsOfPlannedExpenses);
            }

            for (int b = 0; b < userBudgets.Count; b++)
            {
                List<decimal?> budgetDifferences = new List<decimal?>();

                for (int i = 0; i < expenseTypes.Count; i++)
                {
                    var periodExpensesByType = expensesByType[i].
                        Select(x => new ExpenseModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Date = x.Date,
                            Value = x.Value,
                            ExpenseTypeId = x.ExpenseTypeId,
                            UserId = x.UserId,
                        })
                        .Where(e => e.Date > userBudgets[b].StartDate && e.Date < GetEndDate(userBudgets[b]))
                        .ToList();

                    decimal? sumOfExpensesValues = 0.0m;
                    foreach (var expense in periodExpensesByType)
                    {
                        sumOfExpensesValues += expense.Value;
                    }

                    var difference = sumOfExpensesValues - allBudgetsPlannedExpenses[b][i];

                    budgetDifferences.Add(difference);

                }
                differences.Add(budgetDifferences);
            }

            return differences;
        }
    }
}

