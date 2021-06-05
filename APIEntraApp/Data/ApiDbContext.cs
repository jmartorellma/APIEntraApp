using APIEntraApp.Data.Models;
using APIEntraApp.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

            // DESCOMENTAR PARA HACER LAS MIGACIONES
            // Se necesita para la ejecucion pero AspNetUsers ya se crea en el IdentityServer

            //builder.Entity<ApplicationUser>()
            //    .ToTable("AspNetRoleClaims", t => t.ExcludeFromMigrations());
            //builder.Entity<ApplicationRole>()
            //    .ToTable("AspNetRoles", t => t.ExcludeFromMigrations());
            //builder.Entity<ApplicationUser>()
            //    .ToTable("AspNetUserClaims", t => t.ExcludeFromMigrations());
            //builder.Entity<ApplicationUser>()
            //    .ToTable("AspNetUserLogins", t => t.ExcludeFromMigrations());
            //builder.Entity<ApplicationUser>()
            //    .ToTable("AspNetUserRoles", t => t.ExcludeFromMigrations());
            //builder.Entity<ApplicationUser>()
            //   .ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
            //builder.Entity<ApplicationUser>()
            //    .ToTable("AspNetUserTokens", t => t.ExcludeFromMigrations());

            builder.Entity<Message>()
               .HasOne(u => u.Sender)
               .WithMany(u => u.MessagesSent)
               .HasForeignKey(ui => ui.FromId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(s => s.Reciever)
                .WithMany(u => u.MessagesRecived)
                .HasForeignKey(si => si.ToId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Shop>()
               .Property(s => s.Taxes).HasPrecision(18, 2);

            builder.Entity<Shop>()
               .Property(s => s.MinAmountTaxes).HasPrecision(18, 2);

            builder.Entity<User_Shop_Favorite>()
                .HasKey(k => new { k.UserId, k.ShopId });

            builder.Entity<User_Shop_Favorite>()
                .HasOne(u => u.User)
                .WithMany(us => us.User_Shop_Favorites)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Shop_Favorite>()
                .HasOne(s => s.Shop)
                .WithMany(us => us.User_Shop_Favorites)
                .HasForeignKey(si => si.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Shop_Rating>()
                .HasKey(k => new { k.UserId, k.ShopId });

            builder.Entity<User_Shop_Rating>()
                .HasOne(u => u.User)
                .WithMany(us => us.User_Shop_Ratings)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Shop_Rating>()
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

            builder.Entity<User_Product_Favorite>()
               .HasKey(k => new { k.UserId, k.ProductId });

            builder.Entity<User_Product_Favorite>()
                .HasOne(u => u.User)
                .WithMany(us => us.User_Product_Favorites)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Product_Favorite>()
                .HasOne(s => s.Product)
                .WithMany(us => us.User_Product_Favorites)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Product_Rating>()
               .HasOne(u => u.User)
               .WithMany(us => us.User_Product_Ratings)
               .HasForeignKey(ui => ui.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Product_Rating>()
                .HasOne(s => s.Product)
                .WithMany(us => us.User_Product_Ratings)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product_Category>()
              .HasKey(k => new { k.ProductId, k.CategoryId });

            builder.Entity<Product_Category>()
                .HasOne(u => u.Product)
                .WithMany(us => us.Product_Categories)
                .HasForeignKey(ui => ui.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product_Category>()
                .HasOne(s => s.Category)
                .WithMany(us => us.Products_Category)
                .HasForeignKey(si => si.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Product_Cart>()
               .HasOne(u => u.User)
               .WithMany(us => us.User_Products_Cart)
               .HasForeignKey(ui => ui.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User_Product_Cart>()
                .HasOne(s => s.Product)
                .WithMany(us => us.Users_Product_Cart)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Purchase>()
               .Property(p => p.Amount).HasPrecision(18, 2);

            builder.Entity<Purchase_Cart>()
              .HasKey(k => new { k.PurchaseId, k.UserProductCartId });

            builder.Entity<Purchase_Cart>()
                .HasOne(u => u.Purchase)
                .WithMany(us => us.Purchase_Carts)
                .HasForeignKey(ui => ui.PurchaseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Purchase_Cart>()
                .HasOne(s => s.UserProductCart)
                .WithMany(us => us.Purchases_Cart)
                .HasForeignKey(si => si.UserProductCartId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Delivery>()
               .Property(p => p.DeliveryTaxes).HasPrecision(18, 2);
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<User_Shop_Favorite> Users_Shops_Favorites { get; set; }
        public DbSet<User_Shop_Rating> Users_Shops_Ratings { get; set; }
        public DbSet<User_Shop_Locked> Users_Shops_Locked { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User_Product_Favorite> Users_Products_Favorites { get; set; }
        public DbSet<User_Product_Rating> Users_Products_Ratings { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product_Category> Products_Categories { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PurchaseType> PurchaseTypes { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }        
        public DbSet<User_Product_Cart> Users_Products_Cart { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Purchase_Cart> Purchases_Carts { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
    }

}
