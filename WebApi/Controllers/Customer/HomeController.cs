using Application.Interfaces.IServices;
using Application.Dtos.ShoppingCarts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Dtos.Books;

namespace WebApi.Controllers.Customer
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Customer")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;
        private readonly IUserContextService _userContextService;

        public HomeController(IHomeService homeService, IUserContextService userContextService)
        {
            _homeService = homeService;
            _userContextService = userContextService;
        }

        [HttpGet("index")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BookHomeDto>>> Index()
        {
            var books = await _homeService.GetHomeBooksAsync();
            return Ok(books);
        }

        [HttpGet("details/{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDetailsDto>> Details(int bookId)
        {
            var vm = await _homeService.GetBookDetailsAsync(bookId);
            return Ok(vm);
        }

        [HttpPost("add-to-cart")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _userContextService.GetUserId();
            if (!userId.HasValue)
                return Unauthorized("User ID not found in token claims");

            var success = await _homeService.AddToCartAsync(userId.Value, viewModel);

            if (!success)
                return StatusCode(500, "An error occurred while adding the item.");

            return Ok(new { Message = "Item added to the cart successfully!" });
        }
    }
}
