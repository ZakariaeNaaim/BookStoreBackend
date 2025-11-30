using Application.Dtos.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.IServices
{
    public interface IBookService
    {
        Task<IEnumerable<BookListDto>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
