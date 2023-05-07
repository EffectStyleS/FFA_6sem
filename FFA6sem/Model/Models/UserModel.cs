using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace FFA6sem.Model.Models
{
    public class UserModel
    {
        public UserModel()
        {

        }

        public UserModel(User user, UserManager<User> userManager)
        {
            Id       = user.Id;
            UserName = user.UserName;
            GetRole(user, userManager).Wait();
        }

        private async Task GetRole(User user, UserManager<User> userManager)
        {
            try
            {
                IList<string>? roles = await userManager.GetRolesAsync(user);
                Role = roles.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Role = "user";
            }
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
