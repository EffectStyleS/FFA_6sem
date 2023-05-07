using FFA6sem.Model.Models;

namespace FFA6sem.Model.Interfaces
{
    public interface IPlannedIncomesService
    {
        decimal? GetSumOfAllPlannedIncomes(List<PlannedIncomesModel> plannedIncomes);
        bool PlannedIncomesExists(int id);
    }
}
