using Application.Dtos.Common;
using Application.Dtos.Orders;
using Application.Dtos.ShoppingCarts;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Customer")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User ID not found in token claims");
            }
            
            return int.Parse(userIdClaim);
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponseDto>> AddToCart([FromBody] AddToCartDto dto)
        {
            var userId = GetUserId();
            var success = await _cartService.AddToCartAsync(userId, dto.BookId, dto.Quantity);

            if (!success) return BadRequest("Unable to add to cart.");
            return Ok(new SuccessResponseDto("Item added to cart"));
        }

        [HttpGet("index")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShoppingCartDto>> Index()
        {
            var userId = GetUserId();
            var vm = await _cartService.GetCartAsync(userId);
            return Ok(vm);
        }

        [HttpPost("increment/{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto>> IncrementQuantity(int cartId)
        {
            var success = await _cartService.IncrementQuantityAsync(cartId);
            if (!success) return BadRequest("Unable to increment quantity.");
            return Ok(new SuccessResponseDto("Quantity incremented successfully."));
        }

        [HttpPost("decrement/{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto>> DecrementQuantity(int cartId)
        {
            var success = await _cartService.DecrementQuantityAsync(cartId);
            if (!success) return BadRequest("Unable to decrement quantity.");
            return Ok(new SuccessResponseDto("Quantity decremented successfully."));
        }

        [HttpDelete("{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto>> Delete(int cartId)
        {
            var success = await _cartService.DeleteItemAsync(cartId);
            if (!success) return NotFound("Item not found.");
            return Ok(new SuccessResponseDto("Item deleted successfully."));
        }

        [HttpGet("summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShoppingCartDto>> Summary()
        {
            var userId = GetUserId();
            var vm = await _cartService.GetSummaryAsync(userId);
            return Ok(vm);
        }

        [HttpPost("place-order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PlaceOrderResponseDto>> PlaceOrder()
        {
            var userId = GetUserId();
            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            var checkoutUrl = await _cartService.PlaceOrderAsync(userId, domain);

            if (!string.IsNullOrEmpty(checkoutUrl))
            {
                return Ok(new PlaceOrderResponseDto { CheckoutUrl = checkoutUrl });
            }

            return Ok(new PlaceOrderResponseDto { Message = "Order placed successfully." });
        }

        [HttpGet("order-confirmation/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderConfirmationResponseDto>> OrderConfirmation(int orderId)
        {
            await _cartService.ConfirmOrderAsync(orderId);
            return Ok(new OrderConfirmationResponseDto { OrderId = orderId, Status = "Confirmed" });
        }
    }
}
