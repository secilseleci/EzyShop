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
            modelBuilder.Entity<Shop>()
                .HasOne(s => s.Seller)
                .WithMany(u => u.Shops)
                .HasForeignKey(s => s.SellerId)
                .OnDelete(DeleteBehavior.Cascade); // Seller silinirse, mağazalar da silinsin

            // Shop ile Product ilişkisi
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Shop)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.ShopId)
                .OnDelete(DeleteBehavior.Cascade); // Mağaza silinirse, ürünler de silinsin
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<SellerApplication> SellerApplications { get; set; }

    }
}
