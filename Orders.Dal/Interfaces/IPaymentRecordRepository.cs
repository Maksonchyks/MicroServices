using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Domain.Enteties;

namespace Orders.Dal.Interfaces
{
    public interface IPaymentRecordRepository
    {
        Task<PaymentRecord> GetByIdAsync(int paymentId);
        Task<IEnumerable<PaymentRecord>> GetAllAsync();
        Task<IEnumerable<PaymentRecord>> GetByOrderIdAsync(int orderId);
        Task<int> AddAsync(PaymentRecord payment);
        Task<bool> UpdateAsync(PaymentRecord payment);
        Task<bool> DeleteAsync(int paymentId);
    }
}
