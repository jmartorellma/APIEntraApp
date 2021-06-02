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
        }

        public DbSet<Shop> Shops { get; set; }
        public DbSet<User_Shop_Favorites> Users_Shops_Favorites { get; set; }
        public DbSet<User_Shop_Ratings> User_Shop_Ratings { get; set; }
        public DbSet<User_Shop_Locked> User_Shop_Locked { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
