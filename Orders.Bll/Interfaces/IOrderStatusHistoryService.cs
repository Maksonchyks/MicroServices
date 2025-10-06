using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;

namespace Orders.Bll.Interfaces
{
    public interface IOrderStatusHistoryService
    {
        Task<IEnumerable<OrderStatusHistoryDto>> GetAllAsync();
        Task<OrderStatusHistoryDto?> GetByIdAsync(int id);
        Task AddAsync(OrderStatusHistoryDto dto);
        Task UpdateAsync(OrderStatusHistoryDto dto);
        Task DeleteAsync(int id);
    }
}
