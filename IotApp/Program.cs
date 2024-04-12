using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IotApp.Data;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Log current environment
var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
logger.LogInformation("Current environment: {EnvironmentName}", builder.Environment.EnvironmentName);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<ApplicationDbContext>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Configure Kestrel to listen on all interfaces
builder.WebHost.UseUrls("http://*:5264");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    logger.LogInformation("Running in development environment");
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    logger.LogInformation("Running in production or other environment");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
