using Application.Dtos.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetAllAsync();
        Task<UserPermissionsDto?> GetUserPermissionsAsync(int userId);
        Task<bool> ChangePermissionAsync(UserPermissionsDto viewModel);
        Task<bool> LockUnlockAsync(int userId);
    }

}
