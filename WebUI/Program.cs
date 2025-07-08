using DataAccess;
using DataAccess.SeedDatabase;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;
using WebUI.ExtensionMethods;
using WebUI.Middlewares;

var builder = WebApplication.CreateBuilder(args);
//docker
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

if (builder.Environment.IsProduction())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(80);
    });
}
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
//Serilog
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration)
           .WriteTo.Console()
           .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);
});

if (builder.Environment.IsProduction())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8080);
    });
}

// Servis Extension
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureRepositoryRegistration();
builder.Services.ConfigureServiceRegistration();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddRazorPages();


var app = builder.Build();



// Application extension
app.UseHttpsRedirection();
app.UseStaticFiles();
app.ConfigureLocalization();
app.UseRouting();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseExceptionHandler("/Home/Error");
app.UseAuthentication();
app.UseAuthorization();

 

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate(); // ?? Migration’larý otomatik uygula
    await SeedDatabase.SeedDatabaseAsync(scope.ServiceProvider);
}
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
