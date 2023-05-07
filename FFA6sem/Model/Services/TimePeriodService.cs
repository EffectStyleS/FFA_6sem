using DAL.Interfaces;
using FFA6sem.Model.Interfaces;

namespace FFA6sem.Model.Services
{
    public class TimePeriodService : ITimePeriodService
    {
        IDbRepos _db;

        public TimePeriodService(IDbRepos db)
        {
            _db = db;
        }

        public bool TimePeriodExists(int id)
        {
            return _db.TimePeriod.Exists(id);
        }
    }
}
