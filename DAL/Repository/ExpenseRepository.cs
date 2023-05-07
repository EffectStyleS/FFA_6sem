using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class ExpenseRepository : IRepository<Expense>
    {
        private FFAContext _db;

        public ExpenseRepository(FFAContext db)
        {
            this._db = db;
        }
        public void Create(Expense item)
        {
            _db.Expense.Add(item);
        }

        public void Delete(int id)
        {
            Expense expense = _db.Expense.Find(id);
            if (expense != null)
                _db.Expense.Remove(expense);
        }

        public async Task<List<Expense>> GetAll()
        {
            return await _db.Expense.ToListAsync();
        }

        public async Task<Expense> GetItem(int id)
        {
            return await _db.Expense.FindAsync(id);
        }

        public void Update(Expense item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public bool Exists(int id)
        {
            return _db.Expense.Any(e => e.Id == id);
        }
    }
}
