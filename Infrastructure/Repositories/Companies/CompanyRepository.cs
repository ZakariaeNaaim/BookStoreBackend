
using Application.Inerfaces.IRepositories.ICompanies;
using Domain.Entities.Companies;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Companies
{
	public class CompanyRepository : GenericRepository<TbCompany>, ICompanyRepository
	{
		private readonly AppDbContext _context;

		public CompanyRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
        public async Task<IEnumerable<TbCompany>> GetAllAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<TbCompany?> GetByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public void Update(TbCompany entity)
		{
			_context.Companies.Update(entity);
		}
	}
}
