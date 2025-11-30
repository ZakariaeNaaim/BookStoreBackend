using Application.Dtos.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CategoryDto categoryViewModel);
        Task<bool> UpdateAsync(int id, CategoryDto categoryViewModel);
        Task<bool> DeleteAsync(int id);
    }

}
