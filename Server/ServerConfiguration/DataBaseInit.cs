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

namespace IdentityServer.ServerConfiguration
{
    public static class DataBaseInit
    {
        public static void InitializeDatabase(IApplicationBuilder app, IConfiguration configuration)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

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
            }
        }
    }
}
