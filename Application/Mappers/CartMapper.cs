using Application.Dtos.Books;
using Application.Dtos.Orders;
using Domain.Entities.Books;
using Domain.Entities.Identity;
using Domain.Entities.Orders;
using Domain.Entities.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappers
{
    public static class CartMapper
    {
        public static void MapUserToOrder(ApplicationUser user, TbOrder order)
        {
            order.Name = user.Name;
            order.PhoneNumber = user.PhoneNumber;
            order.StreetAddress = user.AddressInfo?.StreetAddress;
            order.City = user.AddressInfo?.City;
            order.State = user.AddressInfo?.State;
            order.PostalCode = user.AddressInfo?.PostalCode;
        }

        public static TbOrderDetail ToOrderDetail(int orderId, TbShoppingCart cart)
        {
            return new TbOrderDetail
            {
                OrderId = orderId,
                BookId = cart.BookId,
                Price = cart.Price,
                Quantity = cart.Quantity
            };
        }

        public static void MapBookToDetails(TbBook book, BookDetailsDto vm)
        {
            vm.BookDetails.Id = book.Id;
            vm.BookDetails.Title = book.Title;
            vm.BookDetails.Author = book.Author;
            vm.BookDetails.ISBN = book.ISBN;
            vm.BookDetails.ListPrice = book.ListPrice;
            vm.BookDetails.Price = book.Price;
            vm.BookDetails.Price50 = book.Price50;
            vm.BookDetails.Price100 = book.Price100;
            vm.BookDetails.Category = book.Category?.Name;
            vm.BookDetails.BookImages = book.BookImages?.Select(img => new TbBookImage
            {
                Id = img.Id,
                ImageUrl = img.ImageUrl,
                IsMainImage = img.IsMainImage
            }).ToList();
        }
       
    }
}
