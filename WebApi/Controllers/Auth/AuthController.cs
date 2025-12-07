using Application.Dtos.Common;
using Application.Dtos.Identity;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<Domain.Entities.Identity.ApplicationUser> _signInManager;
        private readonly IUserContextService _userContextService;

        public AuthController(
            IAuthService authService, 
            SignInManager<Domain.Entities.Identity.ApplicationUser> signInManager,
            IUserContextService userContextService)
        {
            _authService = authService;
            _signInManager = signInManager;
            _userContextService = userContextService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            var response = await _authService.Login(request);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
        {
            var response = await _authService.Register(request);
            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<SuccessResponseDto>> Logout()
        {
            var userId = _userContextService.GetUserId();
            await _authService.Logout(userId?.ToString());
            return Ok(new SuccessResponseDto("Logged out successfully"));
        }

        [HttpGet("external-login")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Controller logic for initiating challenge
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("external-auth-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? "http://localhost:4200/login";
            
            var response = await _authService.HandleExternalLoginCallback(returnUrl, remoteError);

            var userJson = System.Text.Json.JsonSerializer.Serialize(response.User);

            return Redirect($"{returnUrl}?token={response.Token}&user={System.Web.HttpUtility.UrlEncode(userJson)}");
        }
    }
}
