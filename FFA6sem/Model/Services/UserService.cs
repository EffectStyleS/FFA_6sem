using DAL.Entities;
using DAL.Interfaces;
using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace FFA6sem.Model.Services
{
    public class UserService : IUserService
    {
        IDbRepos db;

        public UserService(IDbRepos db)
        {
            this.db = db;
        }

        public async Task<UserModel> GetUserByName(string name, UserManager<User> userManager)
        {
            var helpUser = await db.User.GetAll();

            return helpUser
                .Where(x => x.UserName == name)
                .Select(x => new UserModel(x, userManager))
                .FirstOrDefault();
        }

        //public async Task<IdentityResult> AddToRoleAsync(UserModel userModel, UserManager<User> userManager)
        //{
        //    var helpUsers = await db.User.GetAll();
        //    var result = await userManager.AddToRoleAsync(helpUsers.FirstOrDefault(x => x.UserName == userModel.UserName), userModel.Role);
        //    return result;
        //}

        public async Task SignInAsync(UserModel userModel, SignInManager<User> signInManager)
        {
            try
            {
                var helpUsers = await db.User.GetAll();
                await signInManager.SignInAsync(helpUsers.FirstOrDefault(x => x.UserName == userModel.UserName), false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task<SignInResult> PasswordSignInAsync(UserModel userModel, bool rememberMe, SignInManager<User> signInManager)
        {
            var result = await signInManager.PasswordSignInAsync(userModel.UserName, userModel.Password, rememberMe, false);
            return result;
        }

        public async Task<User> FindByNameAsync(string userName, UserManager<User> userManager)
        {
            var user = await userManager.FindByNameAsync(userName);
            return user;
        }

        public async Task<string> GetUserRole(string userName, UserManager<User> userManager)
        {
            var user = await FindByNameAsync(userName, userManager);
            IList<string>? roles = await userManager.GetRolesAsync(user);
            string? userRole = roles.FirstOrDefault();

            return userRole;
        }

        public async Task<UserModel> GetCurrentUserAsync(HttpContext httpContext, UserManager<User> userManager)
        {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user == null)
            {
                return null;
            }

            UserModel userModel = new UserModel(user, userManager);
            return userModel;
        }
    }
}

