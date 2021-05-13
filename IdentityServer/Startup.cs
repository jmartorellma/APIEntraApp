using System.IO;
using IdentityServer.Data;
using System.Threading.Tasks;
using IdentityServer.Services;
using IdentityServer.Services.Interfaces;
using IdentityServer.Data.Identity;
using IdentityServer.ServerConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http.Features;
using IdentityServer4.Services;

namespace IdentityServer
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

            services.AddTransient<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();
            services.AddSingleton(_configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());

            services.AddScoped<IEmailSenderService, EmailSenderService>();

            services.Configure<FormOptions>(config => {
                config.ValueLengthLimit = int.MaxValue;
                config.MultipartBodyLengthLimit = int.MaxValue;
                config.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(config => 
            {
                config.Password.RequiredLength = 6;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;
                config.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config => 
            {
                config.Cookie.Name = "IdentitySever.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            var assembly = typeof(Startup).Assembly.GetName().Name;

            var filePath = Path.Combine(_env.ContentRootPath, _configuration["CertPath"].ToString());
            var certificate = new X509Certificate2(filePath, _configuration["CertPassword"].ToString());

            services.AddIdentityServer()
                
                .AddAspNetIdentity<ApplicationUser>()

                //.AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
                //.AddInMemoryApiResources(IdentityServerConfiguration.GetApis(_configuration))
                //.AddInMemoryApiScopes(IdentityServerConfiguration.GetScopes(_configuration))
                //.AddInMemoryClients(IdentityServerConfiguration.GetClients(_configuration))
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
                .AddProfileService<ProfileService>()
                .AddSigningCredential(certificate);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                   builder =>
                   {
                       builder.WithOrigins(_configuration["AngularClientEntraAppURL"].ToString())
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                   });
            });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Task.WaitAll(Task.Run(async () => await DataBaseInit.InitializeDatabase(app, _configuration)));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

    }
}
