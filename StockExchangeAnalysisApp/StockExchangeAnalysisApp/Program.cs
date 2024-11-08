using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockExchangeAnalysisApp.Data;
using StockExchangeAnalysisApp.Models; // Added to use custom models
using StockExchangeAnalysisApp.Services; // Added for custom services like IssuerScraperService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity with custom user model if needed, currently left as IdentityUser
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Register the custom IssuerScraperService
builder.Services.AddScoped<IssuerScraperService>();

// Add controllers with views service (this lets the app use MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Run the scraping service during startup
using (var scope = app.Services.CreateScope())
{
    var scraperService = scope.ServiceProvider.GetRequiredService<IssuerScraperService>();
    await scraperService.ScrapeAndStoreIssuerCodesAsync(); // Run this to populate the database
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint(); // Shows detailed errors when working in development
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Redirects errors to a specific page
    app.UseHsts(); // Adds security by telling browsers to use HTTPS
}

app.UseHttpsRedirection(); // Redirects HTTP to HTTPS for secure browsing
app.UseStaticFiles(); // Allows the app to serve static files like images, CSS, JS

app.UseRouting(); // Sets up the app to match incoming requests to the correct routes

app.UseAuthorization(); // Handles user authentication and permissions

// Maps routes so that URLs like /Home/Index map to the HomeController's Index action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Allows Razor Pages to be served if needed

app.Run(); // Starts the app
