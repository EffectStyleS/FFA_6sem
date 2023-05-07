using DAL.Interfaces;
using FFA6sem.Model.Interfaces;

namespace FFA6sem.Model.Services
{
    public class IncomeService : IIncomeService
    {
        IDbRepos _db;

        public IncomeService(IDbRepos db)
        {
            _db = db;
        }

        public bool IncomeExists(int id)
        {
            return _db.Income.Exists(id);
        }
    }
}
