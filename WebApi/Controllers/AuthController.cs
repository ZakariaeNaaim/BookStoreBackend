using Domain.Entities.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService _jwtTokenService;
        
        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, JwtTokenService jwtTokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) return Unauthorized();

            var token = await _jwtTokenService.GenerateToken(user);

            return Ok(new { token, user });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
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
                //Name = request.Name ?? request.Email,
                Name = request.Email,
                //PhoneNumber = request.PhoneNumber,
                PhoneNumber = "14444",
                AddressInfo = new Domain.Entities.Common.AddressInfo
                {
                    //StreetAddress = request.StreetAddress,
                    StreetAddress = "Azhar",
                    //City = request.City,
                    City ="Casablanca",
                    //State = request.State,
                    State ="Morocoo",
                    //PostalCode = request.PostalCode
                    PostalCode = "21255"
                }
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Failed to create user", errors = result.Errors });
            }

            // Add to Customer role by default
            await _userManager.AddToRoleAsync(user, "Customer");

            // Generate token and sign in
            var token = await _jwtTokenService.GenerateToken(user);

            return Ok(new { token, user });
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

            // Instead of redirect, return JSON so Angular can consume it
            return Redirect($"{returnUrl}?token={token}");
        }
    }
}
