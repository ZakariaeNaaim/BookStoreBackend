using Application.Dtos.Books;
using Application.Dtos.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.IServices
{
    public interface IHomeService
    {
        Task<List<BookHomeDto>> GetHomeBooksAsync();
        Task<BookDetailsDto?> GetBookDetailsAsync(int bookId);
        Task<bool> AddToCartAsync(int userId, AddToCartDto viewModel);
    }
}
