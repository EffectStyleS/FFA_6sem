using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class ExpenseTypeRepository : IRepository<ExpenseType>
    {
        private FFAContext _db;

        public ExpenseTypeRepository(FFAContext db)
        {
            this._db = db;
        }

        public void Create(ExpenseType item)
        {
            _db.ExpenseType.Add(item);
        }

        public void Delete(int id)
        {
            ExpenseType expenseTypes = _db.ExpenseType.Find(id);
            if (expenseTypes != null)
                _db.ExpenseType.Remove(expenseTypes);
        }

        public async Task<List<ExpenseType>> GetAll()
        {
            return await _db.ExpenseType.ToListAsync();
        }

        public async Task<ExpenseType> GetItem(int id)
        {
            return await _db.ExpenseType.FindAsync(id);
        }

        public void Update(ExpenseType item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public bool Exists(int id)
        {
            return _db.ExpenseType.Any(et => et.Id == id);
        }
    }
}
