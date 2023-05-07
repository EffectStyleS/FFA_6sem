using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class PlannedIncomesRepository : IRepository<PlannedIncomes>
    {
        private FFAContext _db;

        public PlannedIncomesRepository(FFAContext db)
        {
            this._db = db;
        }
        public void Create(PlannedIncomes item)
        {
            _db.PlannedIncomes.Add(item);
        }

        public void Delete(int id)
        {
            PlannedIncomes plannedIncomes = _db.PlannedIncomes.Find(id);
            if (plannedIncomes != null)
                _db.PlannedIncomes.Remove(plannedIncomes);
        }

        public async Task<List<PlannedIncomes>> GetAll()
        {
            return await _db.PlannedIncomes.ToListAsync();
        }

        public async Task<PlannedIncomes> GetItem(int id)
        {
            return await _db.PlannedIncomes.FindAsync(id);
        }

        public void Update(PlannedIncomes item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public bool Exists(int id)
        {
            return _db.PlannedIncomes.Any(pi => pi.Id == id);
        }
    }
}
