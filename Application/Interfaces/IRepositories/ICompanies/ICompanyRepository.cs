
using Application.Inerfaces.IRepositories.Generic;
using Domain.Entities.Companies;

namespace Application.Inerfaces.IRepositories.ICompanies
{
    public interface ICompanyRepository : IGenericRepository<TbCompany>
    {
        Task<IEnumerable<TbCompany>> GetAllAsync();
        Task<TbCompany?> GetByIdAsync(int id);
        void Update(TbCompany entity);
    }
}
