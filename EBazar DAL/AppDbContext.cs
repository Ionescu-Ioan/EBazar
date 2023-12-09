using EBazar_DAL.Configurations;
using EBazar_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL
{
    public class AppDbContext : IdentityDbContext<
            User,
            Role,
            int,
            IdentityUserClaim<int>,
            UserRole,
            IdentityUserLogin<int>,
            IdentityRoleClaim<int>,
            IdentityUserToken<int>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public AppDbContext()
        {

        }
        public DbSet<ObjectType> ObjectTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderHistory> OrdersHistory { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            modelBuilder.Entity<UserRole>(ur =>
            {
                ur.HasKey(ur => new { ur.UserId, ur.RoleId });

                ur.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId);
                ur.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId);
            });
            modelBuilder.Entity<ObjectType>()
                .HasMany(c => c.Products)
                .WithOne(u => u.ObjectType);
            modelBuilder.Entity<Product>()
                .HasOne(c => c.ObjectType)
                .WithMany(u => u.Products);
            modelBuilder.Entity<Product>()
                .HasMany(c => c.CartItems)
                .WithOne(u => u.Product)
                .HasForeignKey(a => a.ProductId);
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(u => u.Cart)
                .HasForeignKey(a => a.CartId);
        }
    }
}
