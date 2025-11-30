namespace Application.Dtos.Common
{
    public class ErrorDto
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
