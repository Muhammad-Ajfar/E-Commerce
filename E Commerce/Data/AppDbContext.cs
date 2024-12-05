using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<WishList> WishLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply entity configurations
            ConfigureUserEntity(modelBuilder);
            ConfigureAddressEntity(modelBuilder);
            ConfigureCategoryEntity(modelBuilder);
            ConfigureProductEntity(modelBuilder);
            ConfigureCartEntity(modelBuilder);
            ConfigureCartItemEntity(modelBuilder);
            ConfigureOrderEntity(modelBuilder);
            ConfigureOrderItemEntity(modelBuilder);
            ConfigureWishlistEntity(modelBuilder);

            // Seed data
            //SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        // Configuration Methods for Entities

        private void ConfigureUserEntity(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasMany(u => u.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        private void ConfigureAddressEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId);
        }

        private void ConfigureCategoryEntity(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique(); // Ensure category names are unique
        }

        private void ConfigureProductEntity(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); // Precision for decimal type

            modelBuilder.Entity<Product>()
                .Property(p => p.MRP)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        }

        private void ConfigureCartEntity(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId);

        }

        private void ConfigureCartItemEntity(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);
        }

        private void ConfigureOrderEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<Order>()
                .Property(p => p.TotalAmount)
                .HasPrecision(18, 2); // Precision for decimal type
        }

        private void ConfigureOrderItemEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => oi.Id);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<OrderItem>()
                .Property(p => p.TotalPrice)
                .HasPrecision(18, 2); // Precision for decimal type
        }

        private void ConfigureWishlistEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WishList>()
                .HasKey(wl => wl.Id);

            modelBuilder.Entity<WishList>()
                .HasOne(wl => wl.Product)
                .WithMany()
                .HasForeignKey(wl => wl.ProductId);
        }

        // Seed Data
        //private void SeedData(ModelBuilder modelBuilder)
        //{
        //    // Admin User
        //    modelBuilder.Entity<User>().HasData(new User
        //    {
        //        Id = new Guid("c0883f53-c2e0-4b3f-9e25-044dd1a00495"),
        //        Name = "Admin",
        //        Email = "admin@example.com",
        //        Phone = "1234567890",
        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword("adminadmin"),
        //        Role = "Admin"
        //    });
        //}
    }


}
