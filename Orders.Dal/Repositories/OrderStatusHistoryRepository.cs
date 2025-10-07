using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Orders.Dal.Interfaces;
using Orders.Domain.Enteties;
using System.Data;
using MySqlConnector;

namespace Orders.Dal.Repositories
{
    public class OrderStatusHistoryRepository : BaseRepository, IOrderStatusHistoryRepository
    {
        public OrderStatusHistoryRepository(string connectionString) : base(connectionString) { }

        protected new IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

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
                           SELECT LAST_INSERT_ID();";
            return await conn.QuerySingleAsync<int>(sql, status);
        }

        public async Task<bool> UpdateAsync(OrderStatusHistory status)
        {
            using var conn = CreateConnection();
            string sql = @"UPDATE OrderStatusHistory 
                           SET OrderId=@OrderId, Status=@Status, ChangedAt=@ChangedAt
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
