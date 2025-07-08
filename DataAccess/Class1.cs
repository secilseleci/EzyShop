using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

//public class DesignTimeDataContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
//{
//    public ApplicationDbContext CreateDbContext(string[] args)
//    {
//        var configuration = new ConfigurationBuilder()
//             //.AddAppSettings()
//             .Build();

//        var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDbContext)));

//        return new ApplicationDbContext(builder.Options,null);
//    }

//}

//public static class ConfigurationBuilderExtensionMethods
//{
//    public static IConfigurationBuilder AddAppSettings(
//        this IConfigurationBuilder configurationBuilder,
//        string? environmentName = null)
//    {
//        configurationBuilder.AddEnvironmentVariables()
//            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//        if (string.IsNullOrEmpty(environmentName))
//        {
//            environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//        }

//        if (!string.IsNullOrEmpty(environmentName))
//        {
//            configurationBuilder
//                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
//        }

//        return configurationBuilder;
//    }
//}

