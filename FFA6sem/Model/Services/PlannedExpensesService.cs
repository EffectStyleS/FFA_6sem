using DAL.Interfaces;
using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Models;

namespace FFA6sem.Model.Services
{
    public class PlannedExpensesService : IPlannedExpensesService
    {
        IDbRepos _db;

        public PlannedExpensesService(IDbRepos db)
        {
            _db = db;
        }

        public decimal? GetSumOfAllPlannedExpenses(List<PlannedExpensesModel> plannedExpenses)
        {
            decimal? sumOfAllPlannedExpenses = 0.0m;

            foreach (PlannedExpensesModel plannedExpensesModel in plannedExpenses)
            {
                sumOfAllPlannedExpenses += plannedExpensesModel.Sum;
            }

            return sumOfAllPlannedExpenses;
        }

        public bool PlannedExpensesExists(int id)
        {
            return _db.PlannedExpenses.Exists(id);
        }
    }
}
