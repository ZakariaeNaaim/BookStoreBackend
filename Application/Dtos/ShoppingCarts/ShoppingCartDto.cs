
using Domain.Entities.Orders;
using Domain.Entities.ShoppingCart;

namespace Application.Dtos.ShoppingCarts
{
	public class ShoppingCartDto
    {
        public IEnumerable<TbShoppingCart> LstShoppingCarts { get; set; } = [];

        public TbOrder Order { get; set; } = null!; 
    }
}