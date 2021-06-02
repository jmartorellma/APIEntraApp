using IdentityServer.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) 
        {        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.Name)
                .HasMaxLength(256);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.Surname)
                .HasMaxLength(256);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.CreationDate);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.IsActive);
        }
    }
}