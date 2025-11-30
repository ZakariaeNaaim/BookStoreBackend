
using Application.Inerfaces.IRepositories.Generic;
using Domain.Entities.Books;

namespace Application.Inerfaces.IRepositories.IBooks
{
    public interface IBookRepository : IGenericRepository<TbBook>
    {
        Task<TbBook?> GetByIdAsync(int id);
        void Update(TbBook entity);
    }
}
