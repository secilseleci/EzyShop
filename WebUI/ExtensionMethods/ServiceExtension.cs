using Business.Services.Abstract;
using Business.Services.Concrete;
using Core.Interfaces;
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
            options.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDbContext)));
        });
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
            options.LoginPath = "/Auth/Login";
            options.LogoutPath = "/Auth/Logout";

            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            options.Cookie.MaxAge = null;
            options.Cookie.Expiration = null;

        });
        return services;
    }
    public static void ConfigureRepositoryRegistration(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISellerRepository, SellerRepository>();
        //services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IShopRepository, ShopRepository>();
        //services.AddScoped<IOrderRepository, OrderRepository>();
        //services.AddScoped<IOrderItemRepository, OrderItemRepository>();
    }
    public static void ConfigureServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IRazorViewRenderer, RazorViewRenderer>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ISellerService, SellerService>();

        //services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IShopService, ShopService>();
        //  services.AddScoped<IOrderService, OrderService>();
        //services.AddScoped<IOrderItemService, OrderItemService>();
    }
}

