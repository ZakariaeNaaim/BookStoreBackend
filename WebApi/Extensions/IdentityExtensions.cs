using Domain.Entities.Identity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            return services;
        }
    }
}

