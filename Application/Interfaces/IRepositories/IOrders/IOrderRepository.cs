
using Application.Inerfaces.IRepositories.Generic;
using Domain.Entities.Orders;
using System.Linq.Expressions;

namespace Application.Inerfaces.IRepositories.IOrders
{
    public interface IOrderRepository : IGenericRepository<TbOrder>
    {
        Task<TbOrder?> GetByIdAsync(int id, string? includeProperties = null);
        IQueryable<TbOrder> GetAllQueryable(string? includeProperties = null);
        IQueryable<TbOrder> FindAllQueryable(Expression<Func<TbOrder, bool>> filter, string? includeProperties = null);
        void Update(TbOrder order);
        void UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null);
        void UpdateStripePaymentId(int orderId, string sessionId, string paymentIntentId);
    }
}
