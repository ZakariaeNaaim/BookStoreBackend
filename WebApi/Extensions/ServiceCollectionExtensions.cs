using Application.Inerfaces.IRepositories;
using Application.Inerfaces.IRepositories.Generic;
using Application.Inerfaces.IRepositories.IBooks;
using Application.Inerfaces.IRepositories.ICompanies;
using Application.Inerfaces.IRepositories.IIdentity;
using Application.Inerfaces.IRepositories.IOrders;
using Application.Inerfaces.IRepositories.IShoppingCarts;
using Application.Interfaces.IServices;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Data.DbInitializer;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Books;
using Infrastructure.Repositories.Companies;
using Infrastructure.Repositories.Generic;
using Infrastructure.Repositories.Identity;
using Infrastructure.Repositories.Orders;
using Infrastructure.Repositories.ShoppingCarts;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));

            // Infrastructure Services
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IDbInitializer, DbInitializer>();

            // Application Services
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, StripePaymentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IUserContextService, UserContextService>();

            // Repositories
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBookImageRepository, BookImageRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            return services;
        }
    }
}

