using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Orders.Dal.Interfaces;
using Orders.Domain.Enteties;

namespace Orders.Dal.Repositories
{
    public class OrderStatusHistoryRepository : BaseRepository, IOrderStatusHistoryRepository
    {
        public OrderStatusHistoryRepository(string connectionString) : base(connectionString) { }
        public async Task<IEnumerable<OrderStatusHistory>> GetAllAsync()
        {
            using var conn = CreateConnection();
            string sql = "SELECT StatusId, OrderId, Status, ChangedAt FROM OrderStatusHistory";
            return await conn.QueryAsync<OrderStatusHistory>(sql);
        }
        public async Task<OrderStatusHistory> GetByIdAsync(int statusId)
        {
            using var conn = CreateConnection();
            string sql = "SELECT StatusId, OrderId, Status, ChangedAt FROM OrderStatusHistory WHERE StatusId=@Id";
            return await conn.QueryFirstOrDefaultAsync<OrderStatusHistory>(sql, new { Id = statusId });
        }

        public async Task<IEnumerable<OrderStatusHistory>> GetByOrderIdAsync(int orderId)
        {
            using var conn = CreateConnection();
            string sql = "SELECT StatusId, OrderId, Status, ChangedAt FROM OrderStatusHistory WHERE OrderId=@OrderId";
            return await conn.QueryAsync<OrderStatusHistory>(sql, new { OrderId = orderId });
        }

        public async Task<int> AddAsync(OrderStatusHistory status)
        {
            using var conn = CreateConnection();
            string sql = @"INSERT INTO OrderStatusHistory (OrderId, Status, ChangedAt)
                           VALUES (@OrderId, @Status, @ChangedAt);
                           SELECT CAST(SCOPE_IDENTITY() as int)";
            return await conn.QuerySingleAsync<int>(sql, status);
        }

        public async Task<bool> UpdateAsync(OrderStatusHistory status)
        {
            using var conn = CreateConnection();
            string sql = @"UPDATE OrderStatusHistory SET OrderId=@OrderId, Status=@Status, ChangedAt=@ChangedAt
                           WHERE StatusId=@StatusId";
            return await conn.ExecuteAsync(sql, status) > 0;
        }

        public async Task<bool> DeleteAsync(int statusId)
        {
            using var conn = CreateConnection();
            string sql = "DELETE FROM OrderStatusHistory WHERE StatusId=@Id";
            return await conn.ExecuteAsync(sql, new { Id = statusId }) > 0;
        }
    }

}
