namespace Application.Dtos.Orders
{
    public class OrderConfirmationResponseDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = null!;
    }
}
