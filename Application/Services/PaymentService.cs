using Application.Interfaces.IServices;
using Domain.Entities.Orders;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class StripePaymentService : IPaymentService
    {
        public async Task<Session> CreateCheckoutSessionAsync(TbOrder order, IEnumerable<TbOrderDetail> details)
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"https://yourdomain.com/api/admin/orders/payment-confirmation/{order.Id}",
                CancelUrl = $"https://yourdomain.com/api/admin/orders/{order.Id}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in details)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Book.Title
                        }
                    },
                    Quantity = item.Quantity,
                });
            }

            var service = new SessionService();
            var session = await service.CreateAsync(options);
            return session; 
        }

        public async Task<bool> RefundAsync(string paymentIntentId)
        {
            var options = new RefundCreateOptions
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = paymentIntentId,
            };
            var service = new RefundService();
            var refund = await service.CreateAsync(options);
            return refund.Status == "succeeded";
        }

        public async Task<bool> VerifyPaymentAsync(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);
            return session.PaymentStatus.ToLower() == "paid";
        }
    }

}
