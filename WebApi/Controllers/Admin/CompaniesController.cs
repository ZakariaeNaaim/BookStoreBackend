using Application.Dtos.Companies;
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
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyService.GetAllAsync();
            return Ok(companies);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            return Ok(company);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CompanyDto companyViewModel)
        {
            var id = await _companyService.CreateAsync(companyViewModel);
            return Ok(new { success = true, id });
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyDto companyViewModel)
        {
            await _companyService.UpdateAsync(id, companyViewModel);
            return Ok(new { success = true, message = "Company updated successfully!" });
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _companyService.DeleteAsync(id);
            return Ok(new { success = true, message = "Company deleted successfully!" });
        }
    }
}