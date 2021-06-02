﻿using APIEntraApp.Data.Identity;
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

            builder.Entity<Product_Provider>()
              .HasKey(k => new { k.ProductId, k.ProviderId });

            builder.Entity<Product_Provider>()
                .HasOne(u => u.Product)
                .WithMany(us => us.Product_Providers)
                .HasForeignKey(ui => ui.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product_Provider>()
                .HasOne(s => s.Provider)
                .WithMany(us => us.Products_Provider)
                .HasForeignKey(si => si.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);
        }

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
        public DbSet<Product_Provider> Products_Providers { get; set; }
    }

}