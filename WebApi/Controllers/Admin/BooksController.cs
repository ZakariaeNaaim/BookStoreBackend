using Application.Dtos.Books;
using Application.Dtos.Common;
using Application.Inerfaces.IRepositories;
using Application.Inerfaces.IRepositories.Generic;
using Application.Interfaces.IServices;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin
{
	[Route("api/[area]/[controller]")]
	[ApiController]
	[Area("Admin")]
	[Authorize(Roles = nameof(UserRole.Admin))]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<BookListDto>>>> GetAll()
        {
            var books = await _bookService.GetAllAsync();
            return Ok(ApiResponseDto<IEnumerable<BookListDto>>.SuccessResponse(books));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto>> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { success = false, message = $"({id}) is an invalid Id" });

            var result = await _bookService.DeleteAsync(id);
            if (!result)
                return NotFound(new { success = false, message = $"No book found with Id = ({id})" });

            return Ok(new SuccessResponseDto("Book deleted successfully!"));
        }
    }
}
