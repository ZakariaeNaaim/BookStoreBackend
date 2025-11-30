
using Application.Inerfaces.IRepositories.Generic;
using Domain.Entities.Books;

namespace Application.Inerfaces.IRepositories.IBooks
{
    public interface IBookImageRepository : IGenericRepository<TbBookImage>
    {
        void Update(TbBookImage entity);
    }
}
