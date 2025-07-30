using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Infrastructure.Identity
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedRolesAndAdminUserAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();



            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name="Admin" });
            }
            if (!await roleManager.RoleExistsAsync("Agent"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name="Agent" });
            }

            string adminEmail = "golbon.taha@gmail.com";
            if (await userManager.FindByEmailAsync(adminEmail)==null)
            {
                ApplicationUser adminUser = new ApplicationUser
                {
                    UserName=adminEmail,
                    Email=adminEmail,
                    EmailConfirmed=true,
                    PhoneNumber="09938037166",
                    FullName="مدیر سیستم"
                };
                IdentityResult result = await userManager.CreateAsync(adminUser, "Golbon$021");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }

        }
    }
}
