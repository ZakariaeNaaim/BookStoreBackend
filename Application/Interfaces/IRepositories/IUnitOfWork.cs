using Application.Inerfaces.IRepositories.IBooks;
using Application.Inerfaces.IRepositories.ICompanies;
using Application.Inerfaces.IRepositories.IIdentity;
using Application.Inerfaces.IRepositories.IOrders;
using Application.Inerfaces.IRepositories.IShoppingCarts;

namespace Application.Inerfaces.IRepositories
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IBookRepository Book { get; }
        IBookImageRepository BookImage { get; }
        ICompanyRepository Company { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderRepository Order { get; }
        IOrderDetailRepository OrderDetail { get; }
        Task SaveAsync();
    }
}
