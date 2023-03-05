using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace DormProject.Identity.Data
{
    public static class DbSeeder
    {
        public static void SeedUsers(IdentityDbContext db)
        {
            PasswordHasher<AppUser> hasher = new PasswordHasher<AppUser>();

            AppUser admin = new AppUser()
            {
                UserName = "admin@identity.com",
                Id = Guid.NewGuid().ToString("D"),
                Email = "admin@identity.com",
                NormalizedEmail = "admin@identity.com".ToUpper(),
                EmailConfirmed = true,
                NormalizedUserName = "admin@identity.com".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D"),

            };

            admin.PasswordHash = hasher.HashPassword(admin, "adminpass");

            IdentityRole adminRole = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "Admin",
                NormalizedName = "Admin".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            };

            IdentityRole userRole = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "User",
                NormalizedName = "User".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            };


            IdentityUserRole<string> initialAdminRole = new IdentityUserRole<string>()
            {
                RoleId = adminRole.Id,
                UserId = admin.Id
            };

            db.Users.Add(admin);
            db.Roles.Add(adminRole);
            db.Roles.Add(userRole);
            db.UserRoles.Add(initialAdminRole);
            db.SaveChanges();

        }
    }
}
