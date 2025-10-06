using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Dal.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPromotionRepository Promotions { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        IOrderStatusHistoryRepository OrderStatusHistories { get; }
        IPaymentRecordRepository PaymentRecords { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
