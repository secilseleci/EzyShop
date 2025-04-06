using DataAccess;

namespace WebUI.ExtensionMethods;

public static class ApplicationExtension
{
    public static void ConfigureAndCheckMigration(this IApplicationBuilder app)
    {
        ApplicationDbContext context = app
            .ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<ApplicationDbContext>();


    }

    public static void ConfigureLocalization(this WebApplication app)
    {
        app.UseRequestLocalization(options =>
        {
            options.AddSupportedCultures("en-US")
                .AddSupportedUICultures("en-US")
                .SetDefaultCulture("en-US");
        });
    }


}
