using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace identityProjectAPI.Data
{
    public class DataSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", 
                                        Name = "Manager", 
                                        NormalizedName = "MANAGER" },
                new IdentityRole { Id = "4c5e174e-3b0e-446f-86af-483d56fd7210", 
                                   Name = "User", 
                                   NormalizedName = "USER" }
                );
            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {   Id = "cb787015-925f-4f05-b5b8-be190fc62c7f", // primary key
                    Email = "manager@gmail.com",
                    NormalizedEmail = "MANAGER@GMAIL.COM",
                    UserName = "manager@gmail.com",
                    NormalizedUserName = "MANAGER@GMAIL.COM",
                    FirstName = "Admin",
                    LastName = "Admin",
                    PasswordHash = hasher.HashPassword(null, "Admin-123")
                },
                new ApplicationUser
                {   Id = "2ac43ec4-bd63-4cdf-8eed-426b023e6c20", // primary key
                    Email = "user@gmail.com",
                    NormalizedEmail = "USER@GMAIL.COM",
                    UserName = "user@gmail.com",
                    NormalizedUserName = "USER@GMAIL.COM",
                    FirstName = "User",
                    LastName = "User",
                    PasswordHash = hasher.HashPassword(null, "User-123")
                }
            );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserId = "cb787015-925f-4f05-b5b8-be190fc62c7f"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "4c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserId = "2ac43ec4-bd63-4cdf-8eed-426b023e6c20"
                }
            );

        }

    }
}
