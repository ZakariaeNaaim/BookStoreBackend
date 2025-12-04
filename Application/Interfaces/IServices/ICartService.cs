using Application.Dtos.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.IServices
{
    public interface ICartService
    {
        Task<bool> AddToCartAsync(int userId, int bookId, int quantity);
        Task<ShoppingCartDto> GetCartAsync(int userId);
        Task<ShoppingCartDto> GetSummaryAsync(int userId);
        Task<bool> IncrementQuantityAsync(int cartId);
        Task<bool> DecrementQuantityAsync(int cartId);
        Task<bool> DeleteItemAsync(int cartId);
        Task<string?> PlaceOrderAsync(int userId, string domain);
        Task ConfirmOrderAsync(int orderId);
    }

}
