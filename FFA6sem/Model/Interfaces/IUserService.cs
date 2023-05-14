using DAL.Entities;
using FFA6sem.Model.Models;
using Microsoft.AspNetCore.Identity;

namespace FFA6sem.Model.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> GetUserByName(string name, UserManager<User> userManager);
        //Task<IdentityResult> AddToRoleAsync(UserModel userModel, UserManager<User> userManager);
        Task SignInAsync(UserModel userModel, SignInManager<User> signInManager);
        Task<SignInResult> PasswordSignInAsync(UserModel userModel, bool rememberMe, SignInManager<User> signInManager);
        Task<User> FindByNameAsync(string userName, UserManager<User> userManager);
        Task<string> GetUserRole(string userName, UserManager<User> userManager);
        Task<UserModel> GetCurrentUserAsync(HttpContext httpContext, UserManager<User> userManager);
    }
}
