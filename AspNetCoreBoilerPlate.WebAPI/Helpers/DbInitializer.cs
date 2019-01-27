using AspNetCoreBoilerPlate.Domain.HelperClasses;
using AspNetCoreBoilerPlate.Domain.Models;
using AspNetCoreBoilerPlate.EFCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreBoilerPlate.WebAPI.Helpers
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            try
            {
                if (!context.Roles.Any() && !context.Users.Any())
                {
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
                    var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
                    string filePath = Directory.GetCurrentDirectory() + "/Helpers/InitialUser.json";
                    using (StreamReader r = new StreamReader(filePath))
                    {
                        string json = r.ReadToEnd();
                        var items = JsonConvert.DeserializeObject<InitialUserData>(json);
                        foreach (var roleName in items.RoleData)
                        {
                            var role = new AppRole()
                            {
                                Name = roleName.Name
                            };
                            var result = await roleManager.CreateAsync(role);
                        }
                        foreach (var item in items.UserData)
                        {
                            var userName = new AppUser
                            {
                                UserName = item.UserName
                            };
                            string userPassword = item.Password;
                            var createUser = await userManager.CreateAsync(userName, userPassword);
                            if (createUser.Succeeded)
                            {
                                await userManager.AddToRoleAsync(userName, item.Role);
                                await userManager.AddClaimAsync(userName, new Claim(ClaimTypes.Role, item.Role));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
