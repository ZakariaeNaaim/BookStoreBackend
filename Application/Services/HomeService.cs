using Application.Inerfaces.IRepositories;
using Application.Inerfaces.IRepositories.Generic;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.Books;
using Application.Dtos.ShoppingCarts;
using Application.Exceptions;

namespace Application.Services
{
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReadOnlyRepository<BookHomeDto> _readOnlyRepository;

        public HomeService(IUnitOfWork unitOfWork, IReadOnlyRepository<BookHomeDto> readOnlyRepository)
        {
            _unitOfWork = unitOfWork;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<List<BookHomeDto>> GetHomeBooksAsync()
        {
            var books = await _readOnlyRepository.GetAllAsync();
            return books.OrderBy(x => Guid.NewGuid()).ToList(); // random order
        }

        public async Task<BookDetailsDto?> GetBookDetailsAsync(int bookId)
        {
            var bookModel = await _unitOfWork.Book.GetByIdAsync(bookId, "Category,BookImages");
             if (bookModel == null)
                throw new NotFoundException($"Book with ID {bookId} not found.");

            var vm = new BookDetailsDto();
            CartMapper.MapBookToDetails(bookModel!, vm);
            vm.Quantity = 1; // default
            return vm;
        }

        public async Task<bool> AddToCartAsync(int userId, AddToCartDto viewModel)
        {
            var cartFromDb = await _unitOfWork.ShoppingCart.GetAsync(x => x.UserId == userId && x.BookId == viewModel.BookId);

            if (cartFromDb != null)
            {
                cartFromDb.Quantity += viewModel.Quantity;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
                var shoppingCart = new TbShoppingCart
                {
                    UserId = userId,
                    Quantity = viewModel.Quantity,
                    BookId = viewModel.BookId
                };
                await _unitOfWork.ShoppingCart.AddAsync(shoppingCart);
            }

            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
