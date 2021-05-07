using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.ServerConfiguration;
using System;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Data.Identity;
using IdentityServer.Models.Responses;
using IdentityServer.Data;

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

                //if (!context.ApiScopes.Any())
                //{
                //    foreach (var resource in IdentityServerConfiguration.GetScopes(configuration))
                //    {
                //        context.ApiScopes.Add(resource.ToEntity());
                //    }
                //    context.SaveChanges();
                //}

                #endregion IdentityServer

                #region SuperUser

                var contextIdentity = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                contextIdentity.Database.Migrate();

                if (contextIdentity.Users.FirstOrDefault(u => u.Email.Equals("jmartorell@uoc.edu")) == null) 
                {
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    ApplicationUser applicationUser = new ApplicationUser
                    {
                        UserName = "jmartorellma",
                        Email = "jmartorell@uoc.edu",
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
                        var roleresult = userManager.AddToRoleAsync(user, "SuperUser");

                        context.SaveChanges();
                    }
                }              

                #endregion SuperUser
            }
        }
    }
}
