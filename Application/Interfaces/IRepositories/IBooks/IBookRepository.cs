using Application.Inerfaces.IRepositories.Generic;
using Domain.Entities.Books;

namespace Application.Inerfaces.IRepositories.IBooks
{
    public interface IBookRepository : IGenericRepository<TbBook>
    {
        void Update(TbBook entity);
    }
}
