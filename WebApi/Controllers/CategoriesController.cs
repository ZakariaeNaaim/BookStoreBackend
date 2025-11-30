using Application.Dtos.Books;
using Application.Interfaces.IServices;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/admin/[controller]")]
	[ApiController]
	[Authorize(Roles = nameof(UserRole.Admin))]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(new { success = true, data = categories });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { success = false, message = $"No category found with Id = ({id})" });

            return Ok(new { success = true, data = category });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryViewModel)
        {
            var id = await _categoryService.CreateAsync(categoryViewModel);
            return Ok(new { success = true, id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto categoryViewModel)
        {
            var result = await _categoryService.UpdateAsync(id, categoryViewModel);
            if (!result)
                return NotFound(new { success = false, message = $"No category found with Id = ({id})" });

            return Ok(new { success = true, message = "Category updated successfully!" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result)
                return NotFound(new { success = false, message = $"No category found with Id = ({id})" });

            return Ok(new { success = true, message = "Category deleted successfully!" });
        }
    }
}
