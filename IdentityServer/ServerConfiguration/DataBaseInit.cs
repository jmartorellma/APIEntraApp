using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Data;
using IdentityServer.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace IdentityServer.ServerConfiguration
{
    public static class DataBaseInit
    {
        public static async Task InitializeDatabase(IApplicationBuilder app, IConfiguration configuration)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                #region IdentityServer

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityServerConfiguration.GetClients(configuration))
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityServerConfiguration.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in IdentityServerConfiguration.GetApis(configuration))
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in IdentityServerConfiguration.GetScopes(configuration))
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                #endregion IdentityServer

                #region SuperUser

                var contextIdentity = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                contextIdentity.Database.Migrate();                

                if (!contextIdentity.Roles.Any())
                {
                    contextIdentity.Roles.AddRange(new List<ApplicationRole> 
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

                    contextIdentity.SaveChanges();
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

                        contextIdentity.SaveChanges();
                    }
                }

                #endregion SuperUser
            }
        }
    }
}
