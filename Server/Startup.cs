using IdentityServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.ServerConfiguration;
using IdentityServer.Data.Identity;

namespace Server
{
    public class Startup
    {
        public readonly IConfiguration _configuration;
        public readonly IWebHostEnvironment _env;

        public Startup(
            IConfiguration configuration, 
            IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(config => 
            {
                config.UseSqlServer(connectionString);
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(config => 
            {
                config.Password.RequiredLength = 6;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = true;
                config.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config => 
            {
                config.Cookie.Name = "IdentitySever.Cookie";
                config.LoginPath = "/Auth/login";
                config.LogoutPath = "/Auth/logout";
            });

            var assembly = typeof(Startup).Assembly.GetName().Name;

            //var filePath = Path.Combine(_env.ContentRootPath, _configuration["CertPath"].ToString());
            //var certificate = new X509Certificate2(filePath, _configuration["CertPassword"].ToString());

            services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(assembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(assembly));
                })
                //.AddSigningCredential(certificate)
                .AddDeveloperSigningCredential();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DataBaseInit.InitializeDatabase(app, _configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

    }
}
