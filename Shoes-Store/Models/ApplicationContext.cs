using Microsoft.EntityFrameworkCore;
using Shoes_Store.Models.DB;

namespace Shoes_Store.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions <ApplicationContext> options) : base(options) 
        {
            
        }
        

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AccountAdmin> Admins { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartDetail> CartDetails { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductSize> ProductSizes { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<UserSaldo> UserSaldos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>() // satu ke satu
                .HasOne(a => a.User)
                .WithOne(c => c.Cart)
                .HasForeignKey<Cart>(a => a.UserId);

            modelBuilder.Entity<Order>() //satu ke banyak
                .HasOne(u => u.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<Payment>() // banyak ke satu
                .HasMany(p => p.Orders)
                .WithOne(o => o.Payment)
                .HasForeignKey(o => o.PaymentId);

            modelBuilder.Entity<CartDetail>()
                .HasKey(ci => new { ci.ProductId, ci.CartId }); // composite key

            modelBuilder.Entity<CartDetail>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartDetails)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartDetail>()
                .HasOne(ci => ci.Product)
                .WithMany(c => c.CartDetails)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            //Jadi, ketika Anda menghapus sebuah Product, ProductSize yang terkait akan ikut terhapus,
            //dan CartDetail yang terkait dengan ProductSize tersebut juga akan ikut terhapus.

            modelBuilder.Entity<CartDetail>()
                .HasOne(cd => cd.ProductSize)
                .WithMany(c => c.cartDetails)
                .HasForeignKey(cd => cd.ProductSizeId);

            modelBuilder.Entity<Product>()
                .HasOne(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(p => p.Product)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(o => o.Order)
                .WithMany(od => od.OrderDetails)
                .HasForeignKey(o => o.OrderId);

            modelBuilder.Entity<ProductSize>()
                .HasOne(p => p.Product)
                .WithMany(s => s.ProductSizes)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<UserSaldo>()
                .HasOne(u => u.User)
                .WithMany(s => s.UserSaldos)
                .HasForeignKey(u => u.UserId);


            modelBuilder.Entity<AccountAdmin>().HasData(

              new AccountAdmin
              {
                  Id = 1,
                  Name = "Administrator",
                  Pin = HashPassword("123456789")
              }
          );


            base.OnModelCreating(modelBuilder);
        }

        private string HashPassword(string pin)
        {
            // Implementasi hashing password (gunakan BCrypt atau metode secure lainnya)
            return BCrypt.Net.BCrypt.HashPassword(pin);
        }
    }
}
