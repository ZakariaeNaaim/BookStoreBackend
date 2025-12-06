namespace Application.Dtos.Common
{
    public class SuccessResponseDto
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = null!;

        public SuccessResponseDto() { }

        public SuccessResponseDto(string message)
        {
            Message = message;
        }
    }
}
