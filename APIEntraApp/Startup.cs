using APIEntraApp.Data;
using System.Threading.Tasks;
using APIEntraApp.Data.InitData;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Users.Core;
using APIEntraApp.Services.Users;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using APIEntraApp.Services.Carts.Core;
using APIEntraApp.Services.Carts;
using APIEntraApp.Services.Categories.Core;
using APIEntraApp.Services.Categories;
using APIEntraApp.Services.Deliveries.Core;
using APIEntraApp.Services.Deliveries;
using APIEntraApp.Services.PaymentMethods;
using APIEntraApp.Services.PaymentMethods.Core;
using APIEntraApp.Services.PaymentStatuses;
using APIEntraApp.Services.PaymentStatuses.Core;
using APIEntraApp.Services.Products.Core;
using APIEntraApp.Services.Products;
using APIEntraApp.Services.Providers.Core;
using APIEntraApp.Services.Providers;
using APIEntraApp.Services.Purchases;
using APIEntraApp.Services.Purchases.Core;
using APIEntraApp.Services.PurchaseTypes;
using APIEntraApp.Services.PurchaseTypes.Core;
using APIEntraApp.Services.Shops.Core;
using APIEntraApp.Services.Shops;

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

            services.AddSingleton<ICartService, CartService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<IDeliveryService, DeliveryService>();
            //services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<IPaymentMethodService, PaymentMethodService>();
            services.AddSingleton<IPaymentStatusService, PaymentStatusService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IProviderService, ProviderService>();
            services.AddSingleton<IPurchaseService, PurchaseService>();
            services.AddSingleton<IPurchaseTyeService, PurchaseTypeService>();
            services.AddSingleton<IShopService, ShopService>();
            services.AddSingleton<IUserService, UserService>();

            services.AddIdentityCore<ApplicationUser>(config => {
                config.Password.RequiredLength = 6;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;
                config.User.RequireUniqueEmail = true;
            });
            new IdentityBuilder(typeof(ApplicationUser), typeof(ApplicationRole), services)
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddEntityFrameworkStores<ApiDbContext>();

            services.Configure<FormOptions>(options => {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });

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

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), _configuration["ResourcesFolder"].ToString())),
                RequestPath = new PathString($"/{_configuration["ResourcesFolder"]}")
            });
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
