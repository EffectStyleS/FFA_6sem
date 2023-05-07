using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class PlannedExpensesRepository : IRepository<PlannedExpenses>
    {
        private FFAContext _db;

        public PlannedExpensesRepository(FFAContext db)
        {
            this._db = db;
        }
        public void Create(PlannedExpenses item)
        {
            _db.PlannedExpenses.Add(item);
        }

        public void Delete(int id)
        {
            PlannedExpenses plannedExpenses = _db.PlannedExpenses.Find(id);
            if (plannedExpenses != null)
                _db.PlannedExpenses.Remove(plannedExpenses);
        }

        public async Task<List<PlannedExpenses>> GetAll()
        {
            return await _db.PlannedExpenses.ToListAsync();
        }

        public async Task<PlannedExpenses> GetItem(int id)
        {
            return await _db.PlannedExpenses.FindAsync(id);
        }

        public void Update(PlannedExpenses item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public bool Exists(int id)
        {
            return _db.PlannedExpenses.Any(pe => pe.Id == id);
        }
    }
}
