using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class IncomeTypeRepository : IRepository<IncomeType>
    {
        private FFAContext _db;

        public IncomeTypeRepository(FFAContext db)
        {
            this._db = db;
        }

        public void Create(IncomeType item)
        {
            _db.IncomeType.Add(item);
        }

        public void Delete(int id)
        {
            IncomeType incomeTypes = _db.IncomeType.Find(id);
            if (incomeTypes != null)
                _db.IncomeType.Remove(incomeTypes);
        }

        public async Task<List<IncomeType>> GetAll()
        {
            return await _db.IncomeType.ToListAsync();
        }

        public async Task<IncomeType> GetItem(int id)
        {
            return await _db.IncomeType.FindAsync(id);
        }

        public void Update(IncomeType item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public bool Exists(int id)
        {
            return _db.IncomeType.Any(it => it.Id == id);
        }
    }
}
