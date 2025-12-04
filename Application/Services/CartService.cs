using Application.Dtos.ShoppingCarts;
using Application.Inerfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities.Identity;
using Domain.Entities.Orders;
using Domain.Entities.ShoppingCart;
using Domain.Enums;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public CartService(IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<bool> AddToCartAsync(int userId, int bookId, int quantity)
        {
            var existingCart = await _unitOfWork.ShoppingCart
                .GetAsync(c => c.UserId == userId && c.BookId == bookId);

            if (existingCart != null)
            {
                existingCart.Quantity += quantity;
                _unitOfWork.ShoppingCart.Update(existingCart);
            }
            else
            {
                var newCart = new TbShoppingCart
                {
                    UserId = userId,
                    BookId = bookId,
                    Quantity = quantity
                };
                await _unitOfWork.ShoppingCart.AddAsync(newCart);
            }

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<ShoppingCartDto> GetCartAsync(int userId)
        {
            var carts = await _unitOfWork.ShoppingCart.FindAllAsync(x => x.UserId == userId, "Book");

            foreach (var cart in carts)
            {
                cart.Book.BookImages = (await _unitOfWork.BookImage.FindAllAsync(x => x.BookId == cart.BookId && x.IsMainImage)).ToList();
            }

            var vm = new ShoppingCartDto
            {
                LstShoppingCarts = carts,
                Order = new TbOrder()
            };

            CalcOrderTotal(vm);
            return vm;
        }

        public async Task<ShoppingCartDto> GetSummaryAsync(int userId)
        {
            var carts = await _unitOfWork.ShoppingCart.FindAllAsync(x => x.UserId == userId, "Book");
            var user = await _unitOfWork.ApplicationUser.GetByIdAsync(userId);

            var vm = new ShoppingCartDto
            {
                LstShoppingCarts = carts,
                Order = new TbOrder { User = user }
            };

            CartMapper.MapUserToOrder(user, vm.Order);
            CalcOrderTotal(vm);
            return vm;
        }

        public async Task<bool> IncrementQuantityAsync(int cartId)
        {
            var cart = await _unitOfWork.ShoppingCart.GetAsync(x => x.Id == cartId);
            if (cart == null) return false;

            cart.Quantity++;
            _unitOfWork.ShoppingCart.Update(cart);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DecrementQuantityAsync(int cartId)
        {
            var cart = await _unitOfWork.ShoppingCart.GetAsync(x => x.Id == cartId);
            if (cart == null) return false;

            if (cart.Quantity <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
            }
            else
            {
                cart.Quantity--;
                _unitOfWork.ShoppingCart.Update(cart);
            }

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteItemAsync(int cartId)
        {
            var cart = await _unitOfWork.ShoppingCart.GetAsync(x => x.Id == cartId);
            if (cart == null) return false;

            _unitOfWork.ShoppingCart.Remove(cart);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<string?> PlaceOrderAsync(int userId, string domain)
        {
            var carts = await _unitOfWork.ShoppingCart.FindAllAsync(x => x.UserId == userId, "Book");
            var user = await _unitOfWork.ApplicationUser.GetByIdAsync(userId);

            var order = new TbOrder
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                User = user
            };

            SetOrderStatus(order, user);
            CalcOrderTotal(new ShoppingCartDto { LstShoppingCarts = carts, Order = order });

            await _unitOfWork.Order.AddAsync(order);
            await _unitOfWork.SaveAsync();

            var details = new List<TbOrderDetail>();
            foreach (var item in carts)
            {
                var detail = CartMapper.ToOrderDetail(order.Id, item);
                details.Add(detail);
                await _unitOfWork.OrderDetail.AddAsync(detail);
            }

            await _unitOfWork.SaveAsync();

            if (user.CompanyId.GetValueOrDefault() == 0)
            {
                Session session = await _paymentService.CreateCheckoutSessionAsync(order, details);

                order.SessionId = session.Id;
                order.PaymentIntentId = session.PaymentIntentId;

                _unitOfWork.Order.Update(order);
                await _unitOfWork.SaveAsync();

                return session.Url;
            }

            return null;
        }

        public async Task ConfirmOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Order.GetAsync(o => o.Id == orderId, "User");
            if (order == null) return;

            if (order.PaymentStatus != PaymentStatus.ApprovedForDelayedPayment.ToString())
            {
                bool paid = await _paymentService.VerifyPaymentAsync(order.SessionId);
                if (paid)
                {
                    _unitOfWork.Order.UpdateStatus(orderId, PaymentStatus.Approved.ToString(), PaymentStatus.Approved.ToString());
                    await _unitOfWork.SaveAsync();
                }
            }

            var carts = await _unitOfWork.ShoppingCart.FindAllAsync(c => c.UserId == order.UserId);
            _unitOfWork.ShoppingCart.RemoveRange(carts);
            await _unitOfWork.SaveAsync();
        }

        private void SetOrderStatus(TbOrder order, ApplicationUser user)
        {
            if (user.CompanyId.GetValueOrDefault() == 0)
            {
                order.PaymentStatus = PaymentStatus.Pending.ToString();
                order.OrderStatus = PaymentStatus.Pending.ToString();
            }
            else
            {
                order.PaymentStatus = PaymentStatus.ApprovedForDelayedPayment.ToString();
                order.OrderStatus = PaymentStatus.Approved.ToString();
            }
        }

        private void CalcOrderTotal(ShoppingCartDto vm)
        {
            foreach (var cart in vm.LstShoppingCarts)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                vm.Order.OrderTotal += cart.Price * cart.Quantity;
            }
        }

        private decimal GetPriceBasedOnQuantity(TbShoppingCart cart)
        {
            if (cart.Quantity <= 50) return cart.Book.Price;
            if (cart.Quantity <= 100) return cart.Book.Price50;
            return cart.Book.Price100;
        }
    }

}
