using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Domain.Enteties;

namespace Orders.Dal.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<OrderItem> GetByIdAsync(int orderItemId);
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
        Task<int> AddAsync(OrderItem orderItem);
        Task<bool> UpdateAsync(OrderItem orderItem);
        Task<bool> DeleteAsync(int orderItemId);
    }
}
