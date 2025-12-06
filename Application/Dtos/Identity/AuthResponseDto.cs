namespace Application.Dtos.Identity
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public UserInfoDto User { get; set; } = null!;
    }
}
