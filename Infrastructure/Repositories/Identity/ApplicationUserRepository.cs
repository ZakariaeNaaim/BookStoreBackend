
using Application.Inerfaces.IRepositories.IIdentity;
using Domain.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Identity
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly AppDbContext _context;

        public ApplicationUserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ApplicationUser?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
