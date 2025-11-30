using Application.Dtos.Books;
using Application.Inerfaces.IRepositories;
using Application.Inerfaces.IRepositories.IBooks;
using Application.Interfaces.IServices;
using Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllOrderedByDisplayOrderAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                DisplayOrder = c.DisplayOrder
            });
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                DisplayOrder = category.DisplayOrder
            };
        }

        public async Task<int> CreateAsync(CategoryDto categoryViewModel)
        {
            var category = new TbCategory
            {
                Name = categoryViewModel.Name,
                DisplayOrder = categoryViewModel.DisplayOrder
            };

            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveAsync();
            return category.Id;
        }

        public async Task<bool> UpdateAsync(int id, CategoryDto categoryViewModel)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            category.Name = categoryViewModel.Name;
            category.DisplayOrder = categoryViewModel.DisplayOrder;

            _categoryRepository.Update(category);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            _categoryRepository.Remove(category);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }

}
