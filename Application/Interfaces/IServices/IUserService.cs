using Application.Dtos.Identity;
using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserListDto>> GetAllAsync();
        Task<UserPermissionsDto?> GetUserPermissionsAsync(int userId);
        Task<bool> ChangePermissionAsync(UserPermissionsDto viewModel);
        Task<bool> LockUnlockAsync(int userId);
    }
}
