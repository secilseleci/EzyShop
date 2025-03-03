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
            // Seller (AppUser) ile Shop ilişkisi
            modelBuilder.Entity<AppUser>()     
                .HasOne(u => u.Shop)
                .WithOne(s => s.Seller)
                .HasForeignKey<Shop>(s => s.SellerId)
                .OnDelete(DeleteBehavior.Cascade);  

            // 3️⃣ SellerApplication - Shop (Opsiyonel Bire Bir İlişki)
            modelBuilder.Entity<SellerApplication>()
                .HasOne(sa => sa.Shop)
                .WithOne(s => s.SellerApplication)
                .HasForeignKey<SellerApplication>(sa => sa.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3️⃣ SellerApplication - AppUser (Opsiyonel Bire Bir İlişki)
            modelBuilder.Entity<SellerApplication>()
                .HasOne(sa => sa.User)
                .WithOne(u => u.SellerApplication)
                .HasForeignKey<SellerApplication>(sa => sa.UserId)
                .IsRequired(false)   
                .OnDelete(DeleteBehavior.SetNull);   

            // 4️⃣ Shop - Product (Bire Çok İlişki)  
            modelBuilder.Entity<Shop>()
                .HasMany(s => s.Products)
                .WithOne(p => p.Shop)
                .HasForeignKey(p => p.ShopId)
                .OnDelete(DeleteBehavior.Cascade);  

            // ShoppingCart silinirse içindeki ShoppingCartItem'lar da silinsin  
            modelBuilder.Entity<ShoppingCartItem>()
            .HasOne(i => i.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(i => i.CartId)
            .OnDelete(DeleteBehavior.Cascade);  

            //  ShoppingCartItem silinirse Product etkilenmemeli  
            modelBuilder.Entity<ShoppingCartItem>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);



            // 🛒 Order - Customer (AppUser)
            modelBuilder.Entity<Order>()
                .HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.SetNull); // Customer silinirse, Order kalır, CustomerId null olur

            // 🏪 Order - Shop
            modelBuilder.Entity<Order>()
                .HasOne<Shop>()
                .WithMany()
                .HasForeignKey(o => o.ShopId)
                .OnDelete(DeleteBehavior.SetNull); // Shop silinirse, Order kalır, ShopId null olur

            // 👨‍💼 Order - Seller (AppUser)
            modelBuilder.Entity<Order>()
                .HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(o => o.SellerId)
                .OnDelete(DeleteBehavior.SetNull); // Seller silinirse, Order kalır, SellerId null olur

            // 🛍 Order - OrderItem (Bire Çok)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Order silinirse, OrderItems da silinir

            // 🏷 OrderItem - Product
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Ürün silinse bile eski siparişlerden kaybolmaz
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
