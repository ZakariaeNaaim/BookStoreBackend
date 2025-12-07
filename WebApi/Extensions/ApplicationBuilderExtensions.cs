using Infrastructure.Data;
using Infrastructure.Data.DbInitializer;
using Infrastructure.Middleware;
using Stripe;

namespace WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication ConfigureMiddlewarePipeline(this WebApplication app, IConfiguration configuration)
        {
            // Seed Database
            SeedDatabase(app);

            // Development-specific middleware
            if (app.Environment.IsDevelopment())
            {
                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("Content-Security-Policy",
                        "default-src 'self'; connect-src 'self' http://localhost:4200 https://localhost:44381; script-src 'self' 'unsafe-inline' 'unsafe-eval'; style-src 'self' 'unsafe-inline'");
                    await next();
                });

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            // Exception handling middleware (should be early in pipeline)
            app.UseMiddleware<ExceptionMiddleware>();

            // Production-specific middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            // Standard middleware pipeline
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Configure Stripe
            StripeConfiguration.ApiKey = configuration.GetSection("Stripe:SecretKey").Value;

            app.UseRouting();
            app.UseCors("AllowAngular");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            // Map controllers
            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            return app;
        }

        private static void SeedDatabase(WebApplication app)
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IDbInitializer dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.Initialize();
            }
        }
    }
}

