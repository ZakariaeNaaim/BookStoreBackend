
using Domain.Entities.Orders;

namespace Application.Dtos.Orders
{
	public class OrderDto
	{
		public TbOrder Order { get; set; }
		public IEnumerable<TbOrderDetail> OrderDetails { get; set; } = [];
	}
}
