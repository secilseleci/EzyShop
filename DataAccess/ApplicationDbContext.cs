using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using Models.Identity;


namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seller (AppUser) - Shop 
            modelBuilder.Entity<AppUser>()     
                .HasOne(u => u.Shop)
                .WithOne(s => s.Seller)
                .HasForeignKey<Shop>(s => s.SellerId)
                .OnDelete(DeleteBehavior.Cascade);


            // SellerApplication - AppUser 
            modelBuilder.Entity<SellerApplication>()
                .HasOne(sa => sa.User)
                .WithOne(u => u.SellerApplication)
                .HasForeignKey<SellerApplication>(sa => sa.UserId)
                .OnDelete(DeleteBehavior.Cascade );

            // Product - Shop 
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Shop)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            // ShoppingCart - ShoppingCartItem
            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(i => i.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);  

            //  ShoppingCartItem - Product   
            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            // Order - Customer (AppUser)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);


            // Order - Shop
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Shop)
                .WithMany()
                .HasForeignKey(o => o.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            //  OrderItem - Product**
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany() 
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<SellerApplication> SellerApplications { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

    }
}
