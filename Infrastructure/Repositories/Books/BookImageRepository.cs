
using Application.Inerfaces.IRepositories.IBooks;
using Domain.Entities.Books;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories.Books
{
	public class BookImageRepository : GenericRepository<TbBookImage>, IBookImageRepository
	{
		private readonly AppDbContext _context;

		public BookImageRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}

		public void Update(TbBookImage entity)
		{
			_context.BookImages.Update(entity);
		}
	}
}
