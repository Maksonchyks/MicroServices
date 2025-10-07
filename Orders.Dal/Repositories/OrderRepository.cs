using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Orders.Dal.Interfaces;
using Orders.Domain.Enteties;
using System.Data;
using MySqlConnector;

namespace Orders.Dal.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(string connectionString) : base(connectionString) { }

        protected new IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            using var conn = CreateConnection();
            string sql = "SELECT OrderId, CustomerName, OrderDate, TotalAmount, PromotionId FROM Orders WHERE OrderId=@Id";
            return await conn.QueryFirstOrDefaultAsync<Order>(sql, new { Id = orderId });
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            using var conn = CreateConnection();
            string sql = "SELECT OrderId, CustomerName, OrderDate, TotalAmount, PromotionId FROM Orders";
            return await conn.QueryAsync<Order>(sql);
        }

        public async Task<int> AddAsync(Order order)
        {
            using var conn = CreateConnection();
            string sql = @"INSERT INTO Orders (CustomerName, OrderDate, TotalAmount, PromotionId)
                           VALUES (@CustomerName, @OrderDate, @TotalAmount, @PromotionId);
                           SELECT LAST_INSERT_ID();";
            return await conn.QuerySingleAsync<int>(sql, order);
        }

        public async Task<bool> UpdateAsync(Order order)
        {
            using var conn = CreateConnection();
            string sql = @"UPDATE Orders 
                           SET CustomerName=@CustomerName, OrderDate=@OrderDate, TotalAmount=@TotalAmount, PromotionId=@PromotionId
                           WHERE OrderId=@OrderId";
            return await conn.ExecuteAsync(sql, order) > 0;
        }

        public async Task<bool> DeleteAsync(int orderId)
        {
            using var conn = CreateConnection();
            string sql = "DELETE FROM Orders WHERE OrderId=@Id";
            return await conn.ExecuteAsync(sql, new { Id = orderId }) > 0;
        }
    }
}
