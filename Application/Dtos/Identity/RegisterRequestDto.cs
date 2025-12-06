using Application.Dtos.Common;

namespace Application.Dtos.Identity
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public AddressInfoDto? AddressInfo { get; set; }
    }
}
