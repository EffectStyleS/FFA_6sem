using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace DAL.Repository
{
    public class UserRepository : IUserRepository<User>
    {
        private FFAContext _db;

        public UserRepository(FFAContext db)
        {
            this._db = db;
        }

        public async Task<IdentityResult> Create(User item, string password, UserManager<User> userManager)
        {
            var result = await userManager.CreateAsync(item, password);

            return result;
            //_db.User.Add(item);
        }

        public async Task<IdentityResult> Delete(string id, UserManager<User> userManager)
        {
            var user = await userManager.FindByIdAsync(id);
            IdentityResult result = new IdentityResult();
            if (user != null)
            {
                result = await userManager.DeleteAsync(user);
            }
            return result;
        }

        public async Task<List<User>> GetAll()
        {
            return await _db.User.ToListAsync();
        }

        public async Task<User> GetItem(string id, UserManager<User> userManager)
        {
            var user = await userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<IdentityResult> Update(User item, UserManager<User> userManager)
        {
            var result = await userManager.UpdateAsync(item);
            _db.Entry(item).State = EntityState.Modified;

            return result;
        }
    }
}
