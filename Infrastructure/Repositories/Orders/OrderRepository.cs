
using Application.Inerfaces.IRepositories.IOrders;
using Domain.Entities.Orders;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Orders
{
    public class OrderRepository : GenericRepository<TbOrder>, IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TbOrder?> GetByIdAsync(int id, string? includeProperties = null)
        {
            IQueryable<TbOrder> query = _context.Orders;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync(o => o.Id == id);
        }

        public IQueryable<TbOrder> GetAllQueryable(string? includeProperties = null)
        {
            IQueryable<TbOrder> query = _context.Orders;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query;
        }

        public IQueryable<TbOrder> FindAllQueryable(Expression<Func<TbOrder, bool>> filter, string? includeProperties = null)
        {
            IQueryable<TbOrder> query = _context.Orders.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query;
        }

        public void Update(TbOrder order)
        {
            _context.Orders.Update(order);
        }

        public void UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                    order.PaymentStatus = paymentStatus;
            }
        }

        public void UpdateStripePaymentId(int orderId, string sessionId, string paymentIntentId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.SessionId = sessionId;
                order.PaymentIntentId = paymentIntentId;
            }
        }
    }
}