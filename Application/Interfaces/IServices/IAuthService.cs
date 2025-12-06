using Application.Dtos.Identity;

namespace Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Login(LoginRequestDto request);
        Task<AuthResponseDto> Register(RegisterRequestDto request);
        Task<string> GenerateExternalLoginUrl(string provider, string returnUrl);
        Task<AuthResponseDto> HandleExternalLoginCallback(string returnUrl = null, string remoteError = null);
        Task Logout(string userId);
    }
}
