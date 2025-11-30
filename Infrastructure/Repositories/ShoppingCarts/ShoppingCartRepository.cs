
using Application.Inerfaces.IRepositories.IShoppingCarts;
using Domain.Entities.ShoppingCart;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories.ShoppingCarts
{
    public class ShoppingCartRepository : GenericRepository<TbShoppingCart>, IShoppingCartRepository
    {
        private readonly AppDbContext _context;

        public ShoppingCartRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(TbShoppingCart entity)
        {
            _context.ShoppingCarts.Update(entity);
        }
    }
}
