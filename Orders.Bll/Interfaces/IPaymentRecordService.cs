using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;

namespace Orders.Bll.Interfaces
{
    public interface IPaymentRecordService
    {
        Task<IEnumerable<PaymentRecordDto>> GetAllAsync();
        Task<PaymentRecordDto?> GetByIdAsync(int id);
        Task AddAsync(PaymentRecordDto dto);
        Task UpdateAsync(PaymentRecordDto dto);
        Task DeleteAsync(int id);
    }
}
