using DAL.Interfaces;
using FFA6sem.Model.Interfaces;

namespace FFA6sem.Model.Services
{
    public class ExpenseService : IExpenseService
    {
        IDbRepos _db;

        public ExpenseService(IDbRepos db)
        {
            _db = db;
        }

        public bool ExpenseExists(int id)
        {
            return _db.Expense.Exists(id);
        }

    }
}
