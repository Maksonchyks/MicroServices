using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;

namespace Orders.Bll.Interfaces
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDto>> GetAllAsync();
        Task<OrderItemDto?> GetByIdAsync(int id);
        Task AddAsync(OrderItemDto dto);
        Task UpdateAsync(OrderItemDto dto);
        Task DeleteAsync(int id);
    }
}
