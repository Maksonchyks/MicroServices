using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Domain.Enteties;

namespace Orders.Dal.Interfaces
{
    public interface IPromotionRepository
    {
        Task<Promotion> GetByIdAsync(int promotionId);
        Task<IEnumerable<Promotion>> GetAllAsync();
        Task<int> AddAsync(Promotion promotion);
        Task<bool> UpdateAsync(Promotion promotion);
        Task<bool> DeleteAsync(int promotionId);
    }
}
