using APIEntraApp.Data;
using System.Threading.Tasks;
using APIEntraApp.Data.InitData;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using APIEntraApp.Services.Interfaces;
using APIEntraApp.Services;
using APIEntraApp.Data.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace APIEntraApp
{
    public class Startup
    {
        public readonly IConfiguration _configuration;

        public Startup(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApiDbContext>(config =>
            {
                config.UseSqlServer(connectionString);
            });

            services.AddIdentityCore<ApplicationUser>(options => { });
            new IdentityBuilder(typeof(ApplicationUser), typeof(ApplicationRole), services)
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddEntityFrameworkStores<ApiDbContext>();

            services.AddSingleton<IUserService, UserService>();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config =>
                {
                    config.Authority = _configuration["IdentityServerURL"].ToString();
                    config.Audience = _configuration["ApiEntraName"].ToString();
                    config.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                });

            services.AddCors(config => 
            {
                config.AddPolicy("AllowAll", p => 
                {
                    p.AllowAnyOrigin();
                    p.AllowAnyMethod();
                    p.AllowAnyHeader();
                });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Task.WaitAll(Task.Run(async () => await DataBaseInit.InitializeDatabase(app)));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        
    }
}
