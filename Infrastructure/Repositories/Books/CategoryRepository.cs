using Application.Inerfaces.IRepositories.IBooks;
using Domain.Entities.Books;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Books
{
    public class CategoryRepository : GenericRepository<TbCategory>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TbCategory>> GetAllOrderedByDisplayOrderAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        public async Task<TbCategory?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public void Update(TbCategory entity)
        {
            _context.Categories.Update(entity);
        }
    }
}
