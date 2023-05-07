using DAL.Interfaces;
using FFA6sem.Model.Interfaces;

namespace FFA6sem.Model.Services
{
    public class ExpenseTypeService : IExpenseTypeService
    {
        IDbRepos _db;

        public ExpenseTypeService(IDbRepos db)
        {
            _db = db;
        }

        public bool ExpenseTypeExists(int id)
        {
            return _db.ExpenseType.Exists(id);
        }
    }
}
