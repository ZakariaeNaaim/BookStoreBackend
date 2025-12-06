using Application.Dtos.Orders;
using Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappers
{
    public static class OrderMapper
    {
        public static void Map(OrderDto orderViewModel, TbOrder order)
        {
            order.Name = orderViewModel.Order.Name;
            order.StreetAddress = orderViewModel.Order.StreetAddress;
            order.PhoneNumber = orderViewModel.Order.PhoneNumber;
            order.State = orderViewModel.Order.State;
            order.City = orderViewModel.Order.City;

            if (!string.IsNullOrEmpty(orderViewModel.Order.Carrier))
                order.Carrier = orderViewModel.Order.Carrier;

            if (!string.IsNullOrEmpty(orderViewModel.Order.TrackingNumber))
                order.TrackingNumber = orderViewModel.Order.TrackingNumber;
        }

        public static OrderDto ToViewModel(TbOrder order, IEnumerable<TbOrderDetail> details)
        {
            return new OrderDto
            {
                Order = new TbOrder
                {
                    Id = order.Id,
                    Name = order.Name,
                    PhoneNumber = order.PhoneNumber,
                    StreetAddress = order.StreetAddress,
                    City = order.City,
                    State = order.State,
                    PostalCode = order.PostalCode,
                    Carrier = order.Carrier,
                    TrackingNumber = order.TrackingNumber,
                    OrderStatus = order.OrderStatus,
                    PaymentStatus = order.PaymentStatus,
                    ShippingDate = order.ShippingDate,
                    PaymentDueDate = order.PaymentDueDate,
                    SessionId = order.SessionId,
                    PaymentIntentId = order.PaymentIntentId,
                    UserId = order.UserId,
                    OrderDetails = details.ToList() // Assign to TbOrder.OrderDetails
                }
            };
        }

        public static void UpdateEntity(TbOrder order, OrderDto viewModel)
        {
            order.Name = viewModel.Order.Name;
            order.PhoneNumber = viewModel.Order.PhoneNumber;
            order.StreetAddress = viewModel.Order.StreetAddress;
            order.City = viewModel.Order.City;
            order.State = viewModel.Order.State;
            order.PostalCode = viewModel.Order.PostalCode;

            if (!string.IsNullOrEmpty(viewModel.Order.Carrier))
                order.Carrier = viewModel.Order.Carrier;

            if (!string.IsNullOrEmpty(viewModel.Order.TrackingNumber))
                order.TrackingNumber = viewModel.Order.TrackingNumber;
        }
    }
}