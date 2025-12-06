using Application.Dtos.Orders;
using Application.Inerfaces.IRepositories;
using Application.Inerfaces.IRepositories.IOrders;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities.Orders;
using Domain.Enums;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Application.Exceptions;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<OrderDto?> GetDetailsAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, includeProperties: "User");
             if (order == null)
                throw new NotFoundException($"Order with ID {orderId} not found.");

            var details = await _orderDetailRepository.FindAllAsync(x => x.OrderId == orderId, includeProperties: "Book");
            order.OrderDetails = details.ToList();
            
            return new OrderDto { Order = order };
        }

        public async Task<IEnumerable<TbOrder>> GetAllAsync(string status, ClaimsPrincipal user)
        {
            IQueryable<TbOrder> orders;
            if (user.IsInRole(UserRole.Admin.ToString()) || user.IsInRole(UserRole.Employee.ToString()))
            {
                orders = _orderRepository.GetAllQueryable(includeProperties: "User");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)user.Identity!;
                int userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                orders = _orderRepository.FindAllQueryable(x => x.UserId == userId, includeProperties: "User");
            }

            return status switch
            {
                "pending" => orders.Where(x => x.PaymentStatus == PaymentStatus.ApprovedForDelayedPayment.ToString()),
                "inProcess" => orders.Where(x => x.OrderStatus == OrderStatus.Processing.ToString()),
                "completed" => orders.Where(x => x.OrderStatus == OrderStatus.Shipped.ToString()),
                "approved" => orders.Where(x => x.OrderStatus == OrderStatus.Approved.ToString()),
                _ => orders
            };
        }

        public async Task<bool> UpdateOrderDetailsAsync(OrderDto orderViewModel)
        {
            var order = await _orderRepository.GetByIdAsync(orderViewModel.Order.Id);
             if (order == null)
                throw new NotFoundException($"Order with ID {orderViewModel.Order.Id} not found.");

            OrderMapper.Map(orderViewModel, order!);
            _orderRepository.Update(order!);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> StartProcessingAsync(int orderId)
        {
            _orderRepository.UpdateStatus(orderId, OrderStatus.Processing.ToString());
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> ShipOrderAsync(OrderDto orderViewModel)
        {
            var order = await _orderRepository.GetByIdAsync(orderViewModel.Order.Id);
             if (order == null)
                throw new NotFoundException($"Order with ID {orderViewModel.Order.Id} not found.");

            order!.TrackingNumber = orderViewModel.Order.TrackingNumber;
            order.Carrier = orderViewModel.Order.Carrier;
            order.OrderStatus = OrderStatus.Shipped.ToString();
            order.ShippingDate = DateTime.Now;

            if (order.PaymentStatus == PaymentStatus.ApprovedForDelayedPayment.ToString())
                order.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));

            _orderRepository.Update(order);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
             if (order == null)
                throw new NotFoundException($"Order with ID {orderId} not found.");

            if (order!.PaymentStatus == PaymentStatus.Approved.ToString())
            {
                await _paymentService.RefundAsync(order.PaymentIntentId);
                _orderRepository.UpdateStatus(order.Id, OrderStatus.Canceled.ToString(), OrderStatus.Refunded.ToString());
            }
            else
            {
                _orderRepository.UpdateStatus(order.Id, OrderStatus.Canceled.ToString(), OrderStatus.Canceled.ToString());
            }

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<Session?> CreatePaymentSessionAsync(int orderId, string? domain)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, includeProperties: "User");
             if (order == null)
                throw new NotFoundException($"Order with ID {orderId} not found.");

            var details = await _orderDetailRepository.FindAllAsync(x => x.OrderId == orderId, includeProperties: "Book");
            return await _paymentService.CreateCheckoutSessionAsync(order!, details,domain);
        }

        public async Task<bool> ConfirmPaymentAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
             if (order == null)
                throw new NotFoundException($"Order with ID {orderId} not found.");

            if (order!.PaymentStatus == PaymentStatus.ApprovedForDelayedPayment.ToString())
            {
                var paid = await _paymentService.VerifyPaymentAsync(order.SessionId);
                if (paid)
                {
                    _orderRepository.UpdateStripePaymentId(orderId, order.SessionId, order.PaymentIntentId);
                    _orderRepository.UpdateStatus(orderId, order.OrderStatus, PaymentStatus.Approved.ToString() );
                    await _unitOfWork.SaveAsync();
                }
            }
            return true;
        }
    }

}
