using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserRepository<T> where T : class
    {
        Task<List<T>> GetAll();

        Task<T> GetItem(string id, UserManager<T> userManager);
        Task<IdentityResult> Create(T item, string password, UserManager<T> userManager);
        Task<IdentityResult> Update(T item, UserManager<T> userManager);
        Task<IdentityResult> Delete(string id, UserManager<T> userManager);
    }
}
