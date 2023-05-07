using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class BudgetRepository : IRepository<Budget>
    {
        private FFAContext _db;

        public BudgetRepository(FFAContext db)
        {
            this._db = db;
        }
        public void Create(Budget item)
        {
            _db.Budget.Add(item);
        }

        public void Delete(int id)
        {
            Budget budget = _db.Budget.Find(id);
            if (budget != null)
                _db.Budget.Remove(budget);
        }

        public async Task<List<Budget>> GetAll()
        {
            return await _db.Budget.ToListAsync();
        }

        public async Task<Budget> GetItem(int id)
        {
            return await _db.Budget.FindAsync(id);
        }

        public void Update(Budget item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public bool Exists(int id)
        {
            return _db.Budget.Any(b => b.Id == id);
        }
    }
}
