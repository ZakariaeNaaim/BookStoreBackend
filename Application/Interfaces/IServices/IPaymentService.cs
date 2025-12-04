using Domain.Entities.Orders;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.IServices
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSessionAsync(TbOrder order, IEnumerable<TbOrderDetail> details, string? domain);
        Task<bool> RefundAsync(string paymentIntentId);
        Task<bool> VerifyPaymentAsync(string sessionId);
    }

}
