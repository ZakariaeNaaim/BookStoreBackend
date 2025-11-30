using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ShoppingCarts
{
	public class AddToCartDto
	{
		public int BookId { get; set; }

		[Range(1, 1000)]
		public int Quantity { get; set; }
	}
}
