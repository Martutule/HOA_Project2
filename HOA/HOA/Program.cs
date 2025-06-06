using HOA.Models;
using HOA.Repositories;
using HOA.Repositories.Interfaces;
using HOA.Services;
using HOA.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<HOADbContext>();


builder.Services.AddDbContext<HOADbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IResidentsService, ResidentsService>();
builder.Services.AddScoped<IPaymentsService, PaymentsService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddScoped<IAnnouncementsService, AnnouncementsService>();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IIncidentsService, IncidentsService>();
builder.Services.AddScoped<ISupplierContractService, SupplierContractService>();

builder.Services.AddScoped<ISupplierContractRepository, SupplierContractRepository>();
builder.Services.AddScoped<IResidentsRepository, ResidentsRepository>();
builder.Services.AddScoped<IPaymentsRepository, PaymentsRepository>();
builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
builder.Services.AddScoped<IAnnouncementsRepository, AnnouncementsRepository>();
builder.Services.AddScoped<IEventsRepository, EventsRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IIncidentsRepository, IncidentsRepository>();

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.LoginPath = "/Identity/Account/Login";
});

var app = builder.Build();

// Seed roles and users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Define roles
    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Create admin user
    var adminEmail = "admin@example.com";
    var adminPassword = "Admin123!"; // Use a strong password in production
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    // Create normal user
    var userEmail = "user@example.com";
    var userPassword = "User123!"; // Use a strong password in production
    if (await userManager.FindByEmailAsync(userEmail) == null)
    {
        var normalUser = new IdentityUser { UserName = userEmail, Email = userEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(normalUser, userPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(normalUser, "User");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
