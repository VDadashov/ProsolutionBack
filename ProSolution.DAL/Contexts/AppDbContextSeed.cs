using Microsoft.AspNetCore.Identity;
using ProSolution.Core.Entities;
using ProSolution.Core.Entities.Identity;
using ProSolution.Core.Enums;

namespace ProSolution.DAL.Contexts
{
    public static class AppDbContextSeed
    {
        public static async Task SeedDatabaseAsync(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed roles
            foreach (var role in Enum.GetValues(typeof(EUserRole)).Cast<EUserRole>().Select(x => x.ToString()))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed admin
            var adminExists = await userManager.FindByNameAsync("admin");
            if (adminExists == null)
            {
                var userAdmin = new User { UserName = "admin", Email = "admin@admin.com", EmailConfirmed = true,Name="admin",Surname="admin" };
                var result = await userManager.CreateAsync(userAdmin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(userAdmin, EUserRole.Admin.ToString());
                }
                else
                {
                    throw new Exception("Failed to create admin user: " + string.Join("; ", result.Errors.Select(e => e.Description)));
                }
            }

            // Seed moderator
            var modExists = await userManager.FindByNameAsync("SuperAdmin");
            if (modExists == null)
            {
                var userMod = new User { UserName = "SuperAdmin", Email = "mod@mod.com", EmailConfirmed = true,Name="moderator",Surname="moderator" };
                var result = await userManager.CreateAsync(userMod, "Moderator123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(userMod, EUserRole.SuperAdmin.ToString());
                }
                else
                {
                    throw new Exception("Failed to create SuperAdmin user: " + string.Join("; ", result.Errors.Select(e => e.Description)));
                }


            }
            // Seed Author
            var AuthorExists = await userManager.FindByNameAsync("Author");
            if (AuthorExists == null)
            {
                var userMod = new User { UserName = "Author", Email = "Author@Author.com", EmailConfirmed = true, Name = "Author", Surname = "Author" };
                var result = await userManager.CreateAsync(userMod, "Author123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(userMod, EUserRole.Author.ToString());
                }
                else
                {
                    throw new Exception("Failed to create Author user: " + string.Join("; ", result.Errors.Select(e => e.Description)));
                }


            }
            //seed data setting 
            if (!context.Settings.Any())
                {
                      var settings = new List<Setting>
                {
                    new Setting { Key = "SiteTitle", Value = "ProSolution" },
                    new Setting { Key = "SupportEmail", Value = "support@prosolution.com" },
                    new Setting { Key = "PhoneNumber", Value = "+1 (123) 456-7890" },
                    new Setting { Key = "WorkTime", Value = "Mon - Fri: 09:00 - 18:00" },
                    new Setting { Key = "PdfLink", Value = "https://prosolution.com/files/guide.pdf" },
                    new Setting { Key = "Address", Value = "Nərimanov rayonu,Əhməd Rəcəbli küçəsi,1963-cü məhəllə" },
                    new Setting { Key = "DarkLogo", Value = "" },
                    new Setting { Key = "WhiteLogo", Value = "" },
                    new Setting { Key = "FacebookLink", Value = "https://facebook.com/prosolution" },
                    new Setting { Key = "InstagramLink", Value = "https://instagram.com/prosolution" },
                    new Setting { Key = "LinkedInLink", Value = "https://linkedin.com/company/prosolution" },
                    new Setting { Key = "PageSize", Value = "10" },
                    new Setting { Key = "PdfLink", Value = "https://service.prosolution.ltd/wp-content/uploads/2023/12/prosolution-presentation.pdf ." },
                    new Setting { Key = "Discount Price", Value = "29.05.2025" },
                    new Setting { Key = "WhatsUpNumber", Value = "+50554654655" },
                    new Setting { Key = "WhatsUpMessage", Value = "salam meleki" },
                };


                    await context.Settings.AddRangeAsync(settings);
                    await context.SaveChangesAsync();
                }
        }

    }
}
