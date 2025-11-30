
using Application.Inerfaces.IRepositories.Generic;
using Domain.Entities.Orders;
using System.Linq.Expressions;

namespace Application.Inerfaces.IRepositories.IOrders
{
    public interface IOrderDetailRepository : IGenericRepository<TbOrderDetail>
    {
        Task<IEnumerable<TbOrderDetail>> FindAllAsync(Expression<Func<TbOrderDetail, bool>> filter, string? includeProperties = null);
        void Update(TbOrderDetail entity);
    }
}
