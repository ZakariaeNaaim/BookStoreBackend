
using Application.Inerfaces.IRepositories;
using Application.Inerfaces.IRepositories.IBooks;
using Application.Inerfaces.IRepositories.ICompanies;
using Application.Inerfaces.IRepositories.IIdentity;
using Application.Inerfaces.IRepositories.IOrders;
using Application.Inerfaces.IRepositories.IShoppingCarts;
using Application.Inerfaces.IRepositories.Generic;
using Infrastructure.Data;
using Infrastructure.Repositories.Books;
using Infrastructure.Repositories.Companies;
using Infrastructure.Repositories.ShoppingCarts;
using Infrastructure.Repositories.Identity;
using Infrastructure.Repositories.Orders;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Category = new CategoryRepository(_context);
            Book = new BookRepository(_context);
            Company = new CompanyRepository(_context);
            ShoppingCart = new ShoppingCartRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
            Order = new OrderRepository(_context);
            OrderDetail = new OrderDetailRepository(_context);
            BookImage = new BookImageRepository(_context);
        }

        public ICategoryRepository Category { get; private set; }
        public IBookRepository Book { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
		public IOrderRepository Order { get; private set; }
		public IOrderDetailRepository OrderDetail { get; private set; }
        public IBookImageRepository BookImage { get; private set; }

		public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
