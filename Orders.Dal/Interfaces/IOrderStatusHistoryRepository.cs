using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Domain.Enteties;

namespace Orders.Dal.Interfaces
{
    public interface IOrderStatusHistoryRepository
    {
        Task<OrderStatusHistory> GetByIdAsync(int statusId);
        Task<IEnumerable<OrderStatusHistory>> GetAllAsync();
        Task<IEnumerable<OrderStatusHistory>> GetByOrderIdAsync(int orderId);
        Task<int> AddAsync(OrderStatusHistory status);
        Task<bool> UpdateAsync(OrderStatusHistory status);
        Task<bool> DeleteAsync(int statusId);
    }
}
