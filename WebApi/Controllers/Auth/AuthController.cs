using Application.Dtos.Common;
using Application.Dtos.Identity;
using Application.Interfaces.IServices;
using Domain.Entities.Common;
using Domain.Entities.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService _jwtTokenService;
        private readonly ICartService _cartService;
        
        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, JwtTokenService jwtTokenService, ICartService cartService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _cartService = cartService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return Unauthorized(new { message = "Invalid credentials" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) return Unauthorized(new { message = "Invalid credentials" });

            var token = await _jwtTokenService.GenerateToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            var response = new AuthResponseDto
            {
                Token = token,
                User = new UserInfoDto
                {
                    Id = user.Id.ToString(),
                    Email = user.Email!,
                    Name = user.Name,
                    Role = roles.FirstOrDefault() ?? "Customer"
                }
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User with this email already exists" });
            }

            // Create new user
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                AddressInfo = new AddressInfo
                {
                    StreetAddress = request.AddressInfo?.StreetAddress,
                    City = request.AddressInfo?.City,
                    State = request.AddressInfo?.State,
                    PostalCode = request.AddressInfo?.PostalCode
                }
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Failed to create user", errors = result.Errors.Select(e => e.Description) });
            }

            // Add to Customer role by default
            await _userManager.AddToRoleAsync(user, "Customer");

            // Generate token and sign in
            var token = await _jwtTokenService.GenerateToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            var response = new AuthResponseDto
            {
                Token = token,
                User = new UserInfoDto
                {
                    Id = user.Id.ToString(),
                    Email = user.Email!,
                    Name = user.Name,
                    Role = roles.FirstOrDefault() ?? "Customer"
                }
            };

            return Ok(response);
        }


        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<SuccessResponseDto>> Logout()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
            {
                await _cartService.ClearCartAsync(userId);
            }

            return Ok(new SuccessResponseDto("Logged out successfully"));
        }

        [HttpGet("external-login")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("external-auth-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? "http://localhost:4200/login";
            if (remoteError != null)
                return BadRequest(new { message = $"Error from external provider: {remoteError}" });

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return BadRequest(new { message = "Error loading external login information." });

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            ApplicationUser user;

            if (result.Succeeded)
            {
                user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = email, Email = email, Name = email };
                    await _userManager.CreateAsync(user);
                    await _userManager.AddToRoleAsync(user, "Customer");
                }
                await _userManager.AddLoginAsync(user, info);
            }

            var token = await _jwtTokenService.GenerateToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "Customer";

            // Encode user info to pass to Angular
            var userJson = System.Text.Json.JsonSerializer.Serialize(new UserInfoDto
            {
                Id = user.Id.ToString(),
                Email = user.Email!,
                Name = user.Name,
                Role = userRole
            });

            // Instead of redirect, return JSON so Angular can consume it
            return Redirect($"{returnUrl}?token={token}&user={System.Web.HttpUtility.UrlEncode(userJson)}");
        }
    }
}
