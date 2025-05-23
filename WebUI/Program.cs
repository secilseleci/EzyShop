using DataAccess.SeedDatabase;
using Serilog;
using System.Text.Json.Serialization;
using WebUI.ExtensionMethods;
using WebUI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

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
    await SeedDatabase.SeedDatabaseAsync(scope.ServiceProvider);

}
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
