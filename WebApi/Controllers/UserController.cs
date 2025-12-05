using Application.Dtos.Identity;
using Application.Interfaces.IServices;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/admin/[controller]")]
    [Area(nameof(UserRole.Admin))]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}/permissions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPermissions(int id)
        {
            var userPermissions = await _userService.GetUserPermissionsAsync(id);
            if (userPermissions == null)
                return NotFound(new { success = false, message = $"No user found with Id = {id}" });

            return Ok(userPermissions);
        }

        [HttpPost("change-permission")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePermission([FromBody] UserPermissionsDto viewModel)
        {
            var result = await _userService.ChangePermissionAsync(viewModel);
            if (!result)
                return BadRequest(new { success = false, message = "Failed to change user role/permissions." });

            return Ok(new { success = true, message = "User role changed successfully!" });
        }

        [HttpPost("lock-unlock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LockUnlock(int id)
        {
            var result = await _userService.LockUnlockAsync(id);
            if (!result)
                return BadRequest(new { success = false, message = "Failed to lock/unlock user." });

            return Ok(new { success = true, message = "User lock/unlock updated successfully!" });
        }
    }
}