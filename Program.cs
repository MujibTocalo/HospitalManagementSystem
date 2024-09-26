using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration setup
var configuration = builder.Configuration;
var services = builder.Services;

// Database configuration
var connectionString = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container
services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity options
services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
});

// Setup Identity with default options and the application user model
services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<ApplicationDbContext>();

// Configure the application cookie (authentication) to redirect to login page when unauthenticated
// services.ConfigureApplicationCookie(options =>
// {
//     options.LoginPath = "/Identity/Account/Login/";  // Change this if your login page is located elsewhere
// });

// Add controllers and views
services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Enable detailed error pages during development
    app.UseDeveloperExceptionPage();
    // Optional: Automatically run database migrations in development
    // app.UseMigrationsEndPoint();   
}
else
{
    // Production error handler and HSTS setup
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Ensures strict transport security headers are sent
}

// Configure HTTP pipeline
app.UseHttpsRedirection();  // Redirect HTTP requests to HTTPS
app.UseStaticFiles();       // Serve static files

app.UseRouting();           // Enable routing

// Authentication and authorization setup
app.UseAuthentication();
app.UseAuthorization();

// Define the default route pattern
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages
app.MapRazorPages();

// Run the application
app.Run();
