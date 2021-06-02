using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using IdentityServer.Data;
using IdentityServer.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.DbContexts;

namespace IdentityServer.ServerConfiguration
{
    public static class DataBaseInit
    {
        public static async Task InitializeDatabase(IApplicationBuilder app, IConfiguration configuration)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                await serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

                #region IdentityServer

                //var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                //await context.Database.MigrateAsync();
                //if (!context.Clients.Any())
                //{
                //    foreach (var client in IdentityServerConfiguration.GetClients(configuration))
                //    {
                //        await context.Clients.AddAsync(client.ToEntity());
                //    }
                //    await context.SaveChangesAsync();
                //}

                //if (!context.IdentityResources.Any())
                //{
                //    foreach (var resource in IdentityServerConfiguration.GetIdentityResources())
                //    {
                //        await context.IdentityResources.AddAsync(resource.ToEntity());
                //    }
                //    await context.SaveChangesAsync();
                //}

                //if (!context.ApiResources.Any())
                //{
                //    foreach (var resource in IdentityServerConfiguration.GetApis(configuration))
                //    {
                //        await context.ApiResources.AddAsync(resource.ToEntity());
                //    }
                //    await context.SaveChangesAsync();
                //}

                //if (!context.ApiScopes.Any())
                //{
                //    foreach (var resource in IdentityServerConfiguration.GetScopes(configuration))
                //    {
                //        await context.ApiScopes.AddAsync(resource.ToEntity());
                //    }
                //    await context.SaveChangesAsync();
                //}

                #endregion IdentityServer

                #region SuperUser

                var contextIdentity = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
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

                #endregion SuperUser
            }
        }
    }
}
