using APIEntraApp.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using APIEntraApp.Data.Models;

namespace APIEntraApp.Data
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
               .Property(e => e.Name)
               .HasMaxLength(256);

            builder.Entity<ApplicationUser>()
                .Property(e => e.Surname)
                .HasMaxLength(256);

            builder.Entity<ApplicationUser>()
                .Property(e => e.CreationDate);

            builder.Entity<ApplicationUser>()
                .Property(e => e.IsActive);

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
        }

        public DbSet<Shop> Shops { get; set; }
        public DbSet<User_Shop_Favorites> Users_Shops_Favorites { get; set; }
    }
}
