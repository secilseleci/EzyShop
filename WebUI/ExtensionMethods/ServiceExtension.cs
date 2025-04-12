using Business.Services.Abstract;
using Business.Services.Concrete;
using Core.Interfaces;
using Core.Security;
using DataAccess;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Identity;
using WebUI.Services;

namespace WebUI.ExtensionMethods;

public static class ServiceExtension
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("sqlconnection"),
                b => b.MigrationsAssembly("DataAccess"));

            options.EnableSensitiveDataLogging(true);
        });
    }



    public static void ConfigureRepositoryRegistration(this IServiceCollection services)
    {

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISellerRepository, SellerRepository>();

        services.AddScoped<ISellerApplicationRepository, SellerApplicationRepository>();
        services.AddScoped<IShopRepository, ShopRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartLineRepository, CartLineRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();

        //services.AddScoped<CategoryRepository>();
        //services.AddScoped<ICategoryRepository, CachedCategoryRepository>();

        //services.AddScoped<ProductRepository>();
        //services.AddScoped<IProductRepository, CachedProductRepository>();
    }

    public static void ConfigureServiceRegistration(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        services.AddScoped<ISellerApplicationService, SellerApplicationService>();

        services.AddScoped<IShopService, ShopService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICartLineService, CartLineService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<IRazorViewRenderer, RazorViewRenderer>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ISellerService, SellerService>();

    }

    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
        {
             options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@ çşğüöıÇŞĞÜÖİ";
            options.User.RequireUniqueEmail = true; 
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
        
        
        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = false;
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";

            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            options.Cookie.MaxAge = null;
            options.Cookie.Expiration = null;

        });
        return services;
    }
}

