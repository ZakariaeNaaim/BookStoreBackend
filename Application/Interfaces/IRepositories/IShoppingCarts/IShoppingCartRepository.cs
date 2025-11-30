
using Application.Inerfaces.IRepositories.Generic;
using Domain.Entities.ShoppingCart;

namespace Application.Inerfaces.IRepositories.IShoppingCarts
{
    public interface IShoppingCartRepository : IGenericRepository<TbShoppingCart>
    {
        void Update(TbShoppingCart entity);
    }
}
