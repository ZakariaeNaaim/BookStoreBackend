
using Application.Inerfaces.IRepositories.IOrders;
using Domain.Entities.Orders;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Orders
{
    public class OrderDetailRepository : GenericRepository<TbOrderDetail>, IOrderDetailRepository
    {
        private readonly AppDbContext _context;

        public OrderDetailRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TbOrderDetail>> FindAllAsync(Expression<Func<TbOrderDetail, bool>> filter, string? includeProperties = null)
        {
            IQueryable<TbOrderDetail> query = _context.OrderDetails.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.ToListAsync();
        }

        public void Update(TbOrderDetail entity)
        {
            _context.OrderDetails.Update(entity);
        }
    }
}
