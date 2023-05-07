using FFA6sem.Model.Models;

namespace FFA6sem.Model.Interfaces
{
    public interface IPlannedExpensesService
    {
        decimal? GetSumOfAllPlannedExpenses(List<PlannedExpensesModel> plannedExpenses);

        bool PlannedExpensesExists(int id);
    }
}
