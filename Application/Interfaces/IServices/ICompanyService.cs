using Application.Dtos.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.IServices
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllAsync();
        Task<CompanyDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CompanyDto companyViewModel);
        Task<bool> UpdateAsync(int id, CompanyDto companyViewModel);
        Task<bool> DeleteAsync(int id);
    }

}
