using Microsoft.AspNetCore.Identity;

namespace DormProject.Identity.Data
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityDataSeeder(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void SeedData()
        {
            if (this.roleManager.Roles.Any())
            {
                return;
            }

            Task
                .Run(async () =>
                {
                    var adminRole = new IdentityRole("Admin");

                    await this.roleManager.CreateAsync(adminRole);

                    var adminUser = new AppUser
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        SecurityStamp = "RandomSecurityStamp"
                    };

                    await userManager.CreateAsync(adminUser, "adminpass12");

                    await userManager.AddToRoleAsync(adminUser, "Admin");
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
