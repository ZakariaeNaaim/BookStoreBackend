using Application.Dtos.Identity;
using Application.Interfaces.IServices;
using Application.Exceptions;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Application.Dtos.Common;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ICartService _cartService;

        public AuthService(
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, 
            IJwtTokenService jwtTokenService, 
            ICartService cartService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _cartService = cartService;
        }

        public async Task<AuthResponseDto> Login(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
            if (user == null) throw new UnauthorizedException("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false).ConfigureAwait(false);
            if (!result.Succeeded) throw new UnauthorizedException("Invalid credentials");

            return await GenerateAuthResponse(user);
        }

        public async Task<AuthResponseDto> Register(RegisterRequestDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
            if (existingUser != null)
            {
                throw new ValidationException("User with this email already exists");
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                AddressInfo = new Domain.Entities.Common.AddressInfo
                {
                    StreetAddress = request.AddressInfo?.StreetAddress,
                    City = request.AddressInfo?.City,
                    State = request.AddressInfo?.State,
                    PostalCode = request.AddressInfo?.PostalCode
                }
            };

            var result = await _userManager.CreateAsync(user, request.Password).ConfigureAwait(false);
            
            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(e => e.Description));
            }

            await _userManager.AddToRoleAsync(user, "Customer").ConfigureAwait(false);

            return await GenerateAuthResponse(user);
        }

        public async Task Logout(string userId)
        {
             if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int id))
            {
                await _cartService.ClearCartAsync(id).ConfigureAwait(false);
            }
            await _signInManager.SignOutAsync().ConfigureAwait(false);
        }

        public Task<string> GenerateExternalLoginUrl(string provider, string returnUrl)
        {
            // This logic typically resides in Controller because it uses Url.Action
            // But we can generate properties here if passed the redirect URL
            // For now, let's keep the Challenge logic in Controller or return properties
            throw new NotImplementedException("Challenge logic depends on Controller Context");
        }

        public async Task<AuthResponseDto> HandleExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
             if (remoteError != null)
                throw new ApiException($"Error from external provider: {remoteError}");

            var info = await _signInManager.GetExternalLoginInfoAsync().ConfigureAwait(false);
            if (info == null)
                throw new ApiException("Error loading external login information.");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false).ConfigureAwait(false);
            ApplicationUser user;

            if (result.Succeeded)
            {
                user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey).ConfigureAwait(false);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email == null) throw new ApiException("Email not found from external provider.");

                user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = email, Email = email, Name = email };
                    await _userManager.CreateAsync(user).ConfigureAwait(false);
                    await _userManager.AddToRoleAsync(user, "Customer").ConfigureAwait(false);
                }
                await _userManager.AddLoginAsync(user, info).ConfigureAwait(false);
            }

            return await GenerateAuthResponse(user);
        }

        private async Task<AuthResponseDto> GenerateAuthResponse(ApplicationUser user)
        {
            var token = await _jwtTokenService.GenerateToken(user).ConfigureAwait(false);
            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            return new AuthResponseDto
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
        }
    }
}
