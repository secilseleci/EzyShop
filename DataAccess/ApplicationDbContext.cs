using Core.Security;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Abstract;
using Models.Entities.Concrete;
using Models.Identity;


namespace DataAccess;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    private readonly ICurrentUserService _currentUser;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Product
        // Products - Category
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Products - Shop 
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Shop)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.ShopId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product - ProductImages
        modelBuilder.Entity<ProductImage>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.ProductImages)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        //  Product -  CartLines 
        modelBuilder.Entity<CartLine>()
            .HasOne(sci => sci.Product)
            .WithMany(p => p.CartLines)
            .HasForeignKey(sci => sci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        //  Product - OrderItems 
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region Shop
        //  Shop - Seller
        modelBuilder.Entity<Shop>()
            .HasOne(s => s.Seller)
            .WithOne(seller => seller.Shop)
            .HasForeignKey<Shop>(s => s.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Shop - Orders
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Shop)
            .WithMany(s => s.Orders)
            .HasForeignKey(o => o.ShopId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region Customer
        // Customer - AppUser
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.User)
            .WithOne() 
            .HasForeignKey<Customer>(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Customer - Orders
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Customer -  Cart
        modelBuilder.Entity<Cart>()
            .HasOne(sc => sc.Customer)
            .WithOne(c => c.Cart)
            .HasForeignKey<Cart>(sc => sc.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region SellerApplication
        //SellerApplication - Seller
        modelBuilder.Entity<Seller>()
            .HasOne(s => s.SellerApplication)
            .WithOne(sa => sa.Seller)
            .HasForeignKey<Seller>(s => s.SellerApplicationId)
            .OnDelete(DeleteBehavior.Restrict);

        //SellerApplication - AppUser 
        modelBuilder.Entity<SellerApplication>()
            .HasOne(sa => sa.User)
            .WithMany()
            .HasForeignKey(sa => sa.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region Seller

        // Seller - AppUser
        modelBuilder.Entity<Seller>()
            .HasOne(s => s.User)
            .WithOne()
            .HasForeignKey<Seller>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region CartLines

        //  Cart - CartLines
        modelBuilder.Entity<CartLine>()
            .HasOne(cl => cl.Cart)
            .WithMany(c => c.CartLines)
            .HasForeignKey(cl => cl.CartId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region Order

        // Order - OrderItems
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

    }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Customer> Customers { get; set; }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

    public DbSet<SellerApplication> SellerApplications { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartLine> CartLines { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var userId = _currentUser.UserId;

        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    if (entry.Entity.CreatedById == null)
                        entry.Entity.CreatedById = userId;

                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.ModifiedById = userId;

                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;
                    entry.Entity.DeletedById = userId;

                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}
