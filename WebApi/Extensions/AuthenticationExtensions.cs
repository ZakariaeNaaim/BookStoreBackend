using Infrastructure.Configurations.External;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Stripe
            services.Configure<StripeSettings>(configuration.GetSection("Stripe"));

            // Configure Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            })
            .AddFacebook(options =>
            {
                options.AppId = configuration["Facebook:AppId"];
                options.AppSecret = configuration["Facebook:AppSecret"];
            })
            .AddMicrosoftAccount(options =>
            {
                options.ClientId = configuration["Microsoft:ClientId"];
                options.ClientSecret = configuration["Microsoft:ClientSecret"];
            });

            return services;
        }
    }
}

