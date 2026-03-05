using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TourWebApp.Infrastructure.Data.Entities;

namespace TourWebApp.Infrastructure.Data.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            await RoleSeeder(services);
            await SeedAdministrator(services);

            var dataCategory = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedCategories(dataCategory);
            var dataCountry = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedCountrys(dataCountry);

            return app;
        }

        private static async Task RoleSeeder(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Administrator", "Client" };

            foreach (var role in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedAdministrator(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Check if the admin user already exists
            if (await userManager.FindByNameAsync("admin") == null)
            {
                var user = new ApplicationUser
                {
                    FirstName = "admin",
                    LastName = "admin",
                    UserName = "admin",
                    Email = "admin@admin.com",
                    Address = "admin address",           
                    PhoneNumber = "0888888888"
                };

                var result = await userManager.CreateAsync(user, "Admin123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
                
            }
        }
        private static void SeedCategories(ApplicationDbContext dataCategory)
        {
            if (dataCategory.Categories.Any())
            {
                return;
            }

            dataCategory.Categories.AddRange(new[]
            {
              new Category {CategoryName="Laptop"},
              new Category {CategoryName="Computer"},
              new Category {CategoryName="Monitor"},
              new Category {CategoryName="TV"},
              new Category {CategoryName="Mobile phone"},
              new Category {CategoryName="Smart watch"}
            });

            dataCategory.SaveChanges();
        }
        private static void SeedCountrys(ApplicationDbContext dataCountry)
        {
            if (dataCountry.Countrys.Any())
            {
                return;
            }

            dataCountry.Countrys.AddRange(new[]
            {
               new Country {CountryName="Acer" },
               new Country {CountryName="Asus"},
               new Country {CountryName="Apple"},
               new Country {CountryName="Dell"},
               new Country { CountryName = "HP"},
               new Country { CountryName = "Huawei"},
               new Country { CountryName = "SamSung"},
            });

            dataCountry.SaveChanges();
        }
    }
}
