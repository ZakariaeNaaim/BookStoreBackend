using Application.Exceptions;
using Application.Dtos.Books;
using Application.Inerfaces.IRepositories;
using Application.Inerfaces.IRepositories.Generic;
using Application.Inerfaces.IRepositories.IBooks;
using Application.Interfaces.IServices;
using Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookImageRepository _bookImageRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IReadOnlyRepository<BookListDto> _readOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookService(
            IBookRepository bookRepository,
            IBookImageRepository bookImageRepository,
            IFileStorageService fileStorageService,
            IReadOnlyRepository<BookListDto> readOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _bookImageRepository = bookImageRepository;
            _fileStorageService = fileStorageService;
            _readOnlyRepository = readOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BookListDto>> GetAllAsync()
        {
            return await _readOnlyRepository.GetAllAsync();
        }

        public async Task<Domain.Entities.Books.TbBook?> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
             if (book == null)
                throw new NotFoundException($"Book with ID {id} not found.");
            return book;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                 throw new NotFoundException($"Book with ID {id} not found.");

            var images = await _bookImageRepository.FindAllAsync(x => x.BookId == id);
            _bookImageRepository.RemoveRange(images);

            // Delegate file deletion to Infrastructure
            _fileStorageService.DeleteBookImages(id);

            _bookRepository.Remove(book);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }

}
