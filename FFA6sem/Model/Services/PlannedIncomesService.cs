using DAL.Interfaces;
using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Models;

namespace FFA6sem.Model.Services
{
    public class PlannedIncomesService : IPlannedIncomesService
    {
        IDbRepos _db;

        public PlannedIncomesService(IDbRepos db)
        {
            _db = db;
        }

        public decimal? GetSumOfAllPlannedIncomes(List<PlannedIncomesModel> plannedIncomes)
        {
            decimal? sumOfAllPlannedIncomes = 0.0m;

            foreach (PlannedIncomesModel plannedIncomesModel in plannedIncomes)
            {
                sumOfAllPlannedIncomes += plannedIncomesModel.Sum;
            }

            return sumOfAllPlannedIncomes;
        }

        public bool PlannedIncomesExists(int id)
        {
            return _db.PlannedIncomes.Exists(id);
        }
    }
}
