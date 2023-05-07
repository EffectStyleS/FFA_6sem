using DAL.Entities;
using DAL.Interfaces;
using FFA6sem.Model.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace FFA6sem.Data
{
    public static class UsersSeed
    {
        public static async Task UsersSeedAsync(FFAContext context, IServiceProvider serviceProvider)
        {
            var userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();
            try
            {
                context.Database.EnsureCreated();

                if (context.User.Any())
                {
                    return;
                }

                var users = new List<UserModel>()
                {
                    new UserModel()
                    {
                        UserName = "Sergey",
                        Password = "Kursovaya1#",
                        Role     = "user"
                    },

                    new UserModel()
                    {
                        UserName = "Alyosha",
                        Password = "Kursovaya2#",
                        Role     = "user"
                    },

                    new UserModel()
                    {
                        UserName = "Darja",
                        Password = "Kursovaya3#",
                        Role     = "user"
                    },

                    new UserModel()
                    {
                        UserName = "LeBatya",
                        Password = "Kursovaya4#",
                        Role     = "admin"
                    },

                    new UserModel()
                    {
                        UserName = "LeMaman",
                        Password = "Kursovaya5#",
                        Role     = "admin"
                    },
                };

                foreach(var user in users)
                {
                    User newUser = new User
                    {
                        UserName = user.UserName
                    };

                    await userManager.CreateAsync(newUser, user.Password);

                    await userManager.AddToRoleAsync(newUser, user.Role);
                }

                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


    }
}
