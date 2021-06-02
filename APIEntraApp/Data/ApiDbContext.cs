using APIEntraApp.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using APIEntraApp.Data.Models;

namespace APIEntraApp.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // AspNetUsers se crea en el IdentityServer
            builder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers", t => t.ExcludeFromMigrations());

            builder.Entity<User_Shop_Favorites>()
                .HasKey(k => new { k.UserId, k.ShopId });

            builder.Entity<User_Shop_Favorites>()
                .HasOne(u => u.User)
                .WithMany(us => us.User_Shop_Favorites)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Shop_Favorites>()
                .HasOne(s => s.Shop)
                .WithMany(us => us.User_Shop_Favorites)
                .HasForeignKey(si => si.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Shop_Ratings>()
                .HasOne(u => u.User)
                .WithMany(us => us.User_Shop_Ratings)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Shop_Ratings>()
                .HasOne(s => s.Shop)
                .WithMany(us => us.User_Shop_Ratings)
                .HasForeignKey(si => si.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Shop_Locked>()
                .HasKey(k => new { k.UserId, k.ShopId });

            builder.Entity<User_Shop_Locked>()
                .HasOne(u => u.User)
                .WithMany(us => us.User_Shop_Locked)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Shop_Locked>()
                .HasOne(s => s.Shop)
                .WithMany(us => us.User_Shop_Locked)
                .HasForeignKey(si => si.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
               .Property(p => p.Price).HasPrecision(18,2);

            builder.Entity<Product>()
               .Property(p => p.Tax).HasPrecision(18, 2);

            builder.Entity<Product>()
               .Property(p => p.Pvp).HasPrecision(18, 2);

            builder.Entity<User_Product_Favorites>()
               .HasKey(k => new { k.UserId, k.ProductId });

            builder.Entity<User_Product_Favorites>()
                .HasOne(u => u.User)
                .WithMany(us => us.User_Product_Favorites)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Product_Favorites>()
                .HasOne(s => s.Product)
                .WithMany(us => us.User_Product_Favorites)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Product_Ratings>()
               .HasOne(u => u.User)
               .WithMany(us => us.User_Product_Ratings)
               .HasForeignKey(ui => ui.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Product_Ratings>()
                .HasOne(s => s.Product)
                .WithMany(us => us.User_Product_Ratings)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Shop> Shops { get; set; }
        public DbSet<User_Shop_Favorites> Users_Shops_Favorites { get; set; }
        public DbSet<User_Shop_Ratings> Users_Shops_Ratings { get; set; }
        public DbSet<User_Shop_Locked> Users_Shops_Locked { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User_Product_Favorites> Users_Products_Favorites { get; set; }
        public DbSet<User_Product_Ratings> Users_Products_Ratings { get; set; }
        public DbSet<Stock> Stocks { get; set; }
    }

}
