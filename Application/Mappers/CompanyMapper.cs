using Application.Dtos.Companies;
using Application.Dtos.Identity;
using Domain.Entities.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappers
{
    public static class CompanyMapper
    {
        public static CompanyDto ToViewModel(TbCompany company)
        {
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                StreetAddress = company.AddressInfo.StreetAddress,
                City = company.AddressInfo.City,
                State = company.AddressInfo.State,
                PostalCode = company.AddressInfo.PostalCode,
                PhoneNumber = company.PhoneNumber
            };
        }

        public static TbCompany ToEntity(CompanyDto viewModel)
        {
            return new TbCompany
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                AddressInfo = new Domain.Entities.Common.AddressInfo
                {
                    StreetAddress = viewModel.StreetAddress,
                    City = viewModel.City,
                    State = viewModel.State,
                    PostalCode = viewModel.PostalCode
                },
                PhoneNumber = viewModel.PhoneNumber
            };
        }

        public static void UpdateEntity(TbCompany company, CompanyDto viewModel)
        {
            company.Name = viewModel.Name;
            company.AddressInfo = new Domain.Entities.Common.AddressInfo
            {
                StreetAddress = viewModel.StreetAddress,
                City = viewModel.City,
                State = viewModel.State,
                PostalCode = viewModel.PostalCode
            };
            company.PhoneNumber = viewModel.PhoneNumber;
        }

        public static CompanyForPermissionsDto ToCompanyForPermissionsDto(TbCompany company)
        {
            return new CompanyForPermissionsDto
            {
                Id = company.Id,
                Name = company.Name
            };
        }
    }

}
