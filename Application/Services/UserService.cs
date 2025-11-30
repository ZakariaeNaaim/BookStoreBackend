using Application.Dtos.Identity;
using Application.Inerfaces.IRepositories.Generic;
using Application.Inerfaces.IRepositories.ICompanies;
using Application.Inerfaces.IRepositories.IIdentity;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities.Identity;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IReadOnlyRepository<UserListViewModel> _readOnlyRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IApplicationUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository; // assume you have this

        public UserService(
            IReadOnlyRepository<UserListViewModel> readOnlyRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IApplicationUserRepository userRepository,
            ICompanyRepository companyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<UserListViewModel>> GetAllAsync()
        {
            return await _readOnlyRepository.GetAllAsync();
        }

        public async Task<UserPermissionsDto?> GetUserPermissionsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserPermissionsDto
            {
                Id = user.Id,
                Name = user.Name,
                CompanyId = user.CompanyId,
                Role = roles.FirstOrDefault(),
                Roles = _roleManager.Roles
                    .Select(x => new RoleDto { Text = x.Name, Value = x.Name })
                    .ToList(),
                Companies = (await _companyRepository.GetAllAsync()).Select(CompanyMapper.ToCompanyForPermissionsDto).ToList()
            };
        }

        public async Task<bool> ChangePermissionAsync(UserPermissionsDto viewModel)
        {
            var user = await _userRepository.GetByIdAsync(viewModel.Id);
            if (user == null) return false;

            var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (currentRole == null) return false;

            var removeResult = await _userManager.RemoveFromRoleAsync(user, currentRole);
            if (!removeResult.Succeeded) return false;

            if (!await _roleManager.RoleExistsAsync(viewModel.Role)) return false;

            var addResult = await _userManager.AddToRoleAsync(user, viewModel.Role);
            if (!addResult.Succeeded) return false;

            user.CompanyId = viewModel.Role == UserRole.Company.ToString() ? viewModel.CompanyId : null;
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> LockUnlockAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            if (!user.LockoutEnabled) return false; // main admin cannot be locked

            if (user.LockoutEnd > DateTime.Now)
                user.LockoutEnd = DateTime.Now; // unlock
            else
                user.LockoutEnd = DateTime.Now.AddYears(1000); // lock

            await _userRepository.SaveChangesAsync();
            return true;
        }
    }

}
