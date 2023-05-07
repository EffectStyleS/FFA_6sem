using FFA6sem.Model.Models;
namespace FFA6sem.Model.Interfaces
{
    public interface IReportService
    {
        Task<List<List<decimal?>>> TakeDifferenceOfExpensesSum(UserModel user);
    }
}
