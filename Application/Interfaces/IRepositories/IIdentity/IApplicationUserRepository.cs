

using Application.Inerfaces.IRepositories.Generic;
using Domain.Entities.Identity;

namespace Application.Inerfaces.IRepositories.IIdentity
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByIdAsync(int id);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task SaveChangesAsync();
    }
}
