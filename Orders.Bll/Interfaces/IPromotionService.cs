using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;

namespace Orders.Bll.Interfaces
{
    public interface IPromotionService
    {
        Task<IEnumerable<PromotionDto>> GetAllAsync();
        Task<PromotionDto?> GetByIdAsync(int id);
        Task AddAsync(PromotionDto dto);
        Task UpdateAsync(PromotionDto dto);
        Task DeleteAsync(int id);
    }
}
