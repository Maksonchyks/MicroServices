using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;

namespace Orders.Bll.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task AddAsync(OrderDto dto);
        Task UpdateAsync(OrderDto dto);
        Task DeleteAsync(int id);
    }

}
