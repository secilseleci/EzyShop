using Business.Services.Abstract;
using Business.Services.Concrete;
using DataAccess;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using DataAccess.Repositories.Concrete.Cache;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Identity;

namespace WebUI.ExtensionMethods
{
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
            services.AddScoped<ISellerApplicationRepository, SellerApplicationRepository>();
            services.AddScoped<IShopRepository, ShopRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IShoppingCartItemRepository, ShoppingCartItemRepository>();

            //services.AddScoped<CategoryRepository>();
            //services.AddScoped<ICategoryRepository, CachedCategoryRepository>();

            //services.AddScoped<ProductRepository>();
            //services.AddScoped<IProductRepository, CachedProductRepository>();
        }

        public static void ConfigureServiceRegistration(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISellerApplicationService, SellerApplicationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IShopService, ShopService>();


        }

        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                // Türkçe karakterleri UserName'de kabul et
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@ çşğüöıÇŞĞÜÖİ";
                options.User.RequireUniqueEmail = true; // Email benzersiz olsun
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
 
