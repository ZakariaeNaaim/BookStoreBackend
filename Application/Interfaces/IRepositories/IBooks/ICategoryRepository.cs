using Application.Inerfaces.IRepositories.Generic;
using Domain.Entities.Books;

namespace Application.Inerfaces.IRepositories.IBooks
{
    public interface ICategoryRepository : IGenericRepository<TbCategory>
    {
        Task<IEnumerable<TbCategory>> GetAllOrderedByDisplayOrderAsync();
        Task<TbCategory?> GetByIdAsync(int id);
        void Update(TbCategory entity);
    }
}
