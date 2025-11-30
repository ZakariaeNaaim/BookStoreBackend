using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Books;
using Domain.Entities.Orders;
using Domain.Entities.Companies;
using Domain.Entities.Identity;
using Domain.Entities.ShoppingCart;
using Application.Dtos.Books;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TbCategory> Categories { get; set; }
        public DbSet<TbBook> Books { get; set; }   
        public DbSet<TbCompany> Companies { get; set; }
        public DbSet<BookListDto> BookListView { get; set; }
        public DbSet<BookHomeDto> BookHomeView { get; set; }
        public DbSet<TbShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<TbOrder> Orders { get; set; }
        public DbSet<TbOrderDetail> OrderDetails { get; set; }
        public DbSet<TbBookImage> BookImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
