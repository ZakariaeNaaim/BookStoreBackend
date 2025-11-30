using Application.Dtos.Companies;
using Application.Inerfaces.IRepositories;
using Application.Inerfaces.IRepositories.ICompanies;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            return companies.Select(CompanyMapper.ToViewModel);
        }

        public async Task<CompanyDto?> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            return company == null ? null : CompanyMapper.ToViewModel(company);
        }

        public async Task<int> CreateAsync(CompanyDto companyViewModel)
        {
            var company = CompanyMapper.ToEntity(companyViewModel);
            await _companyRepository.AddAsync(company);
            await _unitOfWork.SaveAsync();
            return company.Id;
        }

        public async Task<bool> UpdateAsync(int id, CompanyDto companyViewModel)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null) return false;

            CompanyMapper.UpdateEntity(company, companyViewModel);
            _companyRepository.Update(company);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null) return false;

            _companyRepository.Remove(company);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }

}
