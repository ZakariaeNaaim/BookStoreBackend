using Application.Dtos.Common;
using Application.Dtos.Orders;
using Application.Dtos.ShoppingCarts;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.Customer
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Customer")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IUserContextService _userContextService;

        public CartController(ICartService cartService, IUserContextService userContextService)
        {
            _cartService = cartService;
            _userContextService = userContextService;
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponseDto>> AddToCart([FromBody] AddToCartDto dto)
        {
            var userId = _userContextService.GetUserId();
            if (!userId.HasValue)
                return Unauthorized("User ID not found in token claims");

            var success = await _cartService.AddToCartAsync(userId.Value, dto.BookId, dto.Quantity);

            if (!success) return BadRequest("Unable to add to cart.");
            return Ok(new SuccessResponseDto("Item added to cart"));
        }

        [HttpGet("index")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShoppingCartDto>> Index()
        {
            var userId = _userContextService.GetUserId();
            if (!userId.HasValue)
                return Unauthorized("User ID not found in token claims");

            var vm = await _cartService.GetCartAsync(userId.Value);
            return Ok(vm);
        }

        [HttpPost("increment/{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto>> IncrementQuantity(int cartId)
        {
            await _cartService.IncrementQuantityAsync(cartId);
            return Ok(new SuccessResponseDto("Quantity incremented successfully."));
        }

        [HttpPost("decrement/{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto>> DecrementQuantity(int cartId)
        {
            await _cartService.DecrementQuantityAsync(cartId);
            return Ok(new SuccessResponseDto("Quantity decremented successfully."));
        }

        [HttpDelete("{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto>> Delete(int cartId)
        {
            await _cartService.DeleteItemAsync(cartId);
            return Ok(new SuccessResponseDto("Item deleted successfully."));
        }

        [HttpGet("summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShoppingCartDto>> Summary()
        {
            var userId = _userContextService.GetUserId();
            if (!userId.HasValue)
                return Unauthorized("User ID not found in token claims");

            var vm = await _cartService.GetSummaryAsync(userId.Value);
            return Ok(vm);
        }

        [HttpPost("place-order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PlaceOrderResponseDto>> PlaceOrder()
        {
            var userId = _userContextService.GetUserId();
            if (!userId.HasValue)
                return Unauthorized("User ID not found in token claims");

            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            var checkoutUrl = await _cartService.PlaceOrderAsync(userId.Value, domain);

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
