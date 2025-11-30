using Application.Interfaces.IServices;
using Application.Dtos.ShoppingCarts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Dtos.Books;

namespace WebApi.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Customer")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        private int GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            return int.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        [HttpGet("index")]
        public async Task<ActionResult<IEnumerable<BookHomeDto>>> Index()
        {
            var books = await _homeService.GetHomeBooksAsync();
            return Ok(books);
        }

        [HttpGet("details/{bookId}")]
        public async Task<ActionResult<BookDetailsDto>> Details(int bookId)
        {
            if (bookId <= 0) return BadRequest("Invalid book id.");

            var vm = await _homeService.GetBookDetailsAsync(bookId);
            if (vm == null) return NotFound("Book not found.");

            return Ok(vm);
        }

        [HttpPost("add-to-cart")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            var success = await _homeService.AddToCartAsync(userId, viewModel);

            if (!success)
                return StatusCode(500, "An error occurred while adding the item.");

            return Ok(new { Message = "Item added to the cart successfully!" });
        }
    }
}
