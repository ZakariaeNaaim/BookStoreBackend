namespace Application.Dtos.ShoppingCarts
{
    public class PlaceOrderResponseDto
    {
        public bool Success { get; set; } = true;
        public string? CheckoutUrl { get; set; }
        public string? Message { get; set; }
    }
}
