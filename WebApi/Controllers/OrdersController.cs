using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Application.Inerfaces.IRepositories;
using Domain.Entities.Orders;
using Domain.Enums;

namespace WebApi.Controllers
{
	[Route("api/admin/[controller]")]
	[ApiController]
	[Authorize]
	public class OrdersController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll(string status = "all")
		{
			try
			{
				IQueryable<TbOrder> lstOrders;
				if (User.IsInRole(UserRole.Admin.ToString()) || User.IsInRole(UserRole.Employee.ToString()))
				{
					lstOrders = _unitOfWork.Order.GetAllQueryable(includeProperties: "User");
				}
				else
				{
					ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
					var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
					
					if (string.IsNullOrEmpty(userIdClaim))
					{
						return Unauthorized(new { success = false, message = "User ID not found in token claims" });
					}
					
					int userId = int.Parse(userIdClaim);

					lstOrders = _unitOfWork.Order.FindAllQueryable(x => x.UserId == userId ,includeProperties: "User");
				}

				switch (status)
				{
					case "pending":
						lstOrders = lstOrders.Where(x => x.PaymentStatus == PaymentStatus.ApprovedForDelayedPayment.ToString());
						break;
					case "inProcess":
						lstOrders = lstOrders.Where(x => x.OrderStatus == OrderStatus.Processing.ToString());
						break;
					case "completed":
						lstOrders = lstOrders.Where(x => x.OrderStatus == OrderStatus.Shipped.ToString());
						break;
					case "approved":
						lstOrders = lstOrders.Where(x => x.OrderStatus == OrderStatus.Approved.ToString());
						break;
					default:
						break;
				}

				return Ok(lstOrders);
			}
			catch (Exception ex)
			{
				// Log the exception details (optional)
				return StatusCode(500, new { success = false, message = "An error occurred while retrieving orders." });
			}
		}
	}
}
