using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace APIEntraApp.Data.InitData
{
    public static class DataBaseInit
    {
        public static async Task InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var contextIdentity = serviceScope.ServiceProvider.GetRequiredService<ApiDbContext>();
                await contextIdentity.Database.MigrateAsync();

                if (!contextIdentity.Roles.Any())
                {
                    await contextIdentity.Roles.AddRangeAsync(new List<ApplicationRole>
                    {
                        new ApplicationRole
                        {
                            Name = "SuperUser",
                            NormalizedName = "SUPERUSER"
                        },
                        new ApplicationRole
                        {
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new ApplicationRole
                        {
                            Name = "Shop",
                            NormalizedName = "SHOP"
                        },
                        new ApplicationRole
                        {
                            Name = "Customer",
                            NormalizedName = "CUSTOMER"
                        }
                    });

                    await contextIdentity.SaveChangesAsync();
                }

                if (contextIdentity.Users.FirstOrDefault(u => u.Email.Equals("jmartorellma@uoc.edu")) == null)
                {
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    ApplicationUser applicationUser = new ApplicationUser
                    {
                        UserName = "jmartorellma",
                        Email = "jmartorellma@uoc.edu",
                        PhoneNumber = "666999666",
                        Name = "Jordi",
                        Surname = "Martorell Masip",
                        CreationDate = DateTime.Now,
                        IsActive = true
                    };

                    var result = await userManager.CreateAsync(applicationUser, "Jm123456");
                    if (result.Succeeded)
                    {
                        var user = await userManager.FindByEmailAsync(applicationUser.Email);
                        var roleresult = await userManager.AddToRoleAsync(user, "SuperUser");

                        await contextIdentity.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
