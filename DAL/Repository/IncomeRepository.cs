using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class IncomeRepository : IRepository<Income>
    {
        private FFAContext _db;

        public IncomeRepository(FFAContext db)
        {
            this._db = db;
        }
        public void Create(Income item)
        {
            _db.Income.Add(item);
        }

        public void Delete(int id)
        {
            Income income = _db.Income.Find(id);
            if (income != null)
                _db.Income.Remove(income);
        }

        public async Task<List<Income>> GetAll()
        {
            return await _db.Income.ToListAsync();
        }

        public async Task<Income> GetItem(int id)
        {
            return await _db.Income.FindAsync(id);
        }

        public void Update(Income item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public bool Exists(int id)
        {
            return _db.Income.Any(i => i.Id == id);
        }
    }
}
