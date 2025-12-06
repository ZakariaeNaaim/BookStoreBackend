using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces.IServices;
using Domain.Entities.Orders;
using Domain.Enums;
using Application.Dtos.Orders;
using Application.Dtos.Common;

namespace WebApi.Controllers.Admin
{
	[Route("api/[area]/[controller]")]
	[ApiController]
	[Area("Admin")]
	[Authorize]
	public class OrdersController : ControllerBase
	{
		private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll(string status = "all")
		{
			var orders = await _orderService.GetAllAsync(status, User);
			return Ok(orders);
		}

		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get(int id)
		{
			var orderDto = await _orderService.GetDetailsAsync(id);
			return Ok(orderDto);
		}

		[HttpPut("UpdateOrderDetails")]
		[Authorize(Roles = "Admin,Employee")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateOrderDetails([FromBody] OrderDto orderDto)
		{
			await _orderService.UpdateOrderDetailsAsync(orderDto);
			return Ok(new SuccessResponseDto("Order details updated successfully!"));
		}

		[HttpPost("StartProcessing")]
		[Authorize(Roles = "Admin,Employee")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> StartProcessing([FromBody] Dictionary<string, int> payload)
		{
			if (!payload.ContainsKey("id"))
			{
				return BadRequest(new { success = false, message = "Order ID is required." });
			}

			await _orderService.StartProcessingAsync(payload["id"]);
			return Ok(new SuccessResponseDto("Order status updated successfully!"));
		}

		[HttpPost("ShipOrder")]
		[Authorize(Roles = "Admin,Employee")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> ShipOrder([FromBody] OrderDto orderDto)
		{
			await _orderService.ShipOrderAsync(orderDto);
			return Ok(new SuccessResponseDto("Order shipped successfully!"));
		}

		[HttpPost("CancelOrder")]
		[Authorize(Roles = "Admin,Employee")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CancelOrder([FromBody] Dictionary<string, int> payload)
		{
			if (!payload.ContainsKey("id"))
			{
				return BadRequest(new { success = false, message = "Order ID is required." });
			}

			await _orderService.CancelOrderAsync(payload["id"]);
			return Ok(new SuccessResponseDto("Order canceled successfully!"));
		}
	}
}
