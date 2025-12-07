using Domain.Entities.Identity;

namespace Application.Interfaces.IServices
{
    public interface IJwtTokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
