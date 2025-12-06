
using Application.Inerfaces.IRepositories.IBooks;
using Domain.Entities.Books;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories.Books
{
    public class BookRepository : GenericRepository<TbBook>, IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        public void Update(TbBook entity)
        {
            _context.Books.Update(entity);
        }
    }
}
