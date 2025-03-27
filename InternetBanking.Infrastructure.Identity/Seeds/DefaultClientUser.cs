
using InternetBanking.Core.Application.Enums;
using InternetBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace InternetBanking.Infrastructure.Identity.Seeds
{
    public class DefaultClientUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser defaultUser = new()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "client@email.com",
                IdentificationNumer = "002-0034009-3",
                UserName = "clientUser",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true,
            };

            if(userManager.Users.All(u => u.Id != defaultUser.Id))
            {

                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    var createResult = await userManager.CreateAsync(defaultUser, "1234");
                    var addRoleResult =  await userManager.AddToRoleAsync(defaultUser, Roles.Client.ToString());
                    Console.WriteLine("xd");
                }

            }

            
        }
    }
}
