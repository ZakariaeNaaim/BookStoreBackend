using Application.Dtos.Common;
using Application.Dtos.Identity;
using Application.Interfaces.IServices;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin
{
    [Route("api/[area]/[controller]")]
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

        [HttpGet("roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<RoleDto>>> GetRoles()
        {
            var roles = await _userService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id:int}/permissions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPermissions(int id)
        {
            var userPermissions = await _userService.GetUserPermissionsAsync(id);
            return Ok(userPermissions);
        }

        [HttpPost("change-permission")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto>> ChangePermission([FromBody] UserPermissionsDto viewModel)
        {
            await _userService.ChangePermissionAsync(viewModel);
            return Ok(new SuccessResponseDto("User role changed successfully!"));
        }

        [HttpPost("lock-unlock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto>> LockUnlock(int id)
        {
            await _userService.LockUnlockAsync(id);
            return Ok(new SuccessResponseDto("User lock/unlock updated successfully!"));
        }
    }
}