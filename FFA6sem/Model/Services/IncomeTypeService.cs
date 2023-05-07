using DAL.Interfaces;
using FFA6sem.Model.Interfaces;

namespace FFA6sem.Model.Services
{
    public class IncomeTypeService : IIncomeTypeService
    {

        IDbRepos _db;

        public IncomeTypeService(IDbRepos db)
        {
            _db = db;
        }

        public bool IncomeTypeExists(int id)
        {
            return _db.IncomeType.Exists(id);
        }
    }
}
