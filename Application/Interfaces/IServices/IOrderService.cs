using Application.Dtos.Orders;
using Domain.Entities.Orders;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Application.Interfaces.IServices
{
    public interface IOrderService
    {
        Task<OrderDto?> GetDetailsAsync(int orderId);
        Task<IEnumerable<TbOrder>> GetAllAsync(string status, ClaimsPrincipal user);
        Task<bool> UpdateOrderDetailsAsync(OrderDto orderViewModel);
        Task<bool> StartProcessingAsync(int orderId);
        Task<bool> ShipOrderAsync(OrderDto orderViewModel);
        Task<bool> CancelOrderAsync(int orderId);
        Task<Session?> CreatePaymentSessionAsync(int orderId, string? domain);
        Task<bool> ConfirmPaymentAsync(int orderId);
    }

}
