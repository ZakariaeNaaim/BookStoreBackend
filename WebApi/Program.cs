using Application.Inerfaces.IRepositories;
using Application.Inerfaces.IRepositories.Generic;
using Domain.Entities.Identity;
using Infrastructure.Configurations.External;
using Infrastructure.Data;
using Infrastructure.Data.DbInitializer;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Generic;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

#region Custom Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
#endregion

#region Session Management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
#endregion

#region External Login Facebook/Microsoft Authentication
//builder.Services.AddAuthentication().AddFacebook(options =>
//{
//    options.AppId = builder.Configuration.GetSection("Facebook:AppId").Value;
//    options.AppSecret = builder.Configuration.GetSection("Facebook:AppSecret").Value;
//});

//builder.Services.AddAuthentication().AddMicrosoftAccount(options =>
//{
//    options.ClientId = builder.Configuration.GetSection("Microsoft:ClientId").Value;
//    options.ClientSecret = builder.Configuration.GetSection("Microsoft:ClientSecret").Value;
//});
#endregion

var app = builder.Build();

SeedDatabase();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Value;

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

void SeedDatabase()
{
    using (IServiceScope scope = app.Services.CreateScope())
    {
        IDbInitializer dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
