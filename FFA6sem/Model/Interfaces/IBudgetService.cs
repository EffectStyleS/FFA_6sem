using FFA6sem.Model.Models;

namespace FFA6sem.Model.Interfaces
{
    public interface IBudgetService
    {
        Task CreateProperties(BudgetModel budget);
        DateTime GetEndDate(BudgetModel budget);
        Task SavePDF(BudgetModel budget, string filePath);
        bool BudgetExists(int id);
    }
}
