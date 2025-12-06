using Application.Interfaces.IServices;
using Domain.Entities.Orders;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StripePaymentService : IPaymentService
    {
        public async Task<Session> CreateCheckoutSessionAsync(TbOrder order, IEnumerable<TbOrderDetail> details, string? domain)
        {
            // Hardcode frontend URL for Stripe redirects
            var frontendUrl = "http://localhost:4200/";

            var options = new SessionCreateOptions
            {
                // Redirect to Angular routes after payment
                SuccessUrl = frontendUrl + $"customer/order-confirmation/{order.Id}",
                CancelUrl = frontendUrl + "customer/cart",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in details)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),  // Stripe uses cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item?.Book?.Title ?? "Book"
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
