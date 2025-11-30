using Domain.Entities.Books;

namespace Domain.Entities.Orders
{
	public class TbOrderDetail
	{
		public int Id { get; set; }

		public int OrderId { get; set; }
		public TbOrder Order { get; set; } = null!;

		public int BookId { get; set; }
		public TbBook Book { get; set; } = null!;

		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
}
