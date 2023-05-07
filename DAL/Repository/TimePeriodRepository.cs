using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class TimePeriodRepository : IRepository<TimePeriod>
    {
        private FFAContext _db;

        public TimePeriodRepository(FFAContext db)
        {
            this._db = db;
        }
        public void Create(TimePeriod item)
        {
            _db.TimePeriod.Add(item);
        }

        public void Delete(int id)
        {
            TimePeriod timePeriod = _db.TimePeriod.Find(id);
            if (timePeriod != null)
                _db.TimePeriod.Remove(timePeriod);
        }

        public async Task<List<TimePeriod>> GetAll()
        {
            return await _db.TimePeriod.ToListAsync();
        }

        public async Task<TimePeriod> GetItem(int id)
        {
            return await _db.TimePeriod.FindAsync(id);
        }

        public void Update(TimePeriod item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public bool Exists(int id)
        {
            return _db.TimePeriod.Any(t => t.Id == id);
        }
    }
}
