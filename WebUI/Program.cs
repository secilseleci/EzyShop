using DataAccess;
using DataAccess.SeedDatabase;

using Serilog;
using System.Text.Json.Serialization;
using WebUI.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Serilog
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration)
           .WriteTo.Console()
           .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);
});
// Servis Extension

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureRepositoryRegistration();
builder.Services.ConfigureServiceRegistration();
builder.Services.AddAutoMapper(typeof(Program));
 
builder.Services.AddRazorPages();
var app = builder.Build();
 
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{      
    await SeedDatabase.SeedDatabaseAsync(scope.ServiceProvider);
    
}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.ConfigureLocalization();

app.Run();
