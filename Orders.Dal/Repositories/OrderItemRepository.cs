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
    public class OrderItemRepository : BaseRepository, IOrderItemRepository
    {
        public OrderItemRepository(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            using var conn = CreateConnection();
            string sql = "SELECT OrderItemId, OrderId, ProductName, Quantity, Price FROM OrderItem";
            return await conn.QueryAsync<OrderItem>(sql);
        }

        public async Task<OrderItem> GetByIdAsync(int orderItemId)
        {
            using var conn = CreateConnection();
            string sql = "SELECT OrderItemId, OrderId, ProductName, Quantity, Price FROM OrderItem WHERE OrderItemId=@Id";
            return await conn.QueryFirstOrDefaultAsync<OrderItem>(sql, new { Id = orderItemId });
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            using var conn = CreateConnection();
            string sql = "SELECT OrderItemId, OrderId, ProductName, Quantity, Price FROM OrderItem WHERE OrderId=@OrderId";
            return await conn.QueryAsync<OrderItem>(sql, new { OrderId = orderId });
        }

        public async Task<int> AddAsync(OrderItem orderItem)
        {
            using var conn = CreateConnection();
            string sql = @"INSERT INTO OrderItem (OrderId, ProductName, Quantity, Price)
                           VALUES (@OrderId, @ProductName, @Quantity, @Price);
                           SELECT CAST(SCOPE_IDENTITY() as int)";
            return await conn.QuerySingleAsync<int>(sql, orderItem);
        }

        public async Task<bool> UpdateAsync(OrderItem orderItem)
        {
            using var conn = CreateConnection();
            string sql = @"UPDATE OrderItem SET OrderId=@OrderId, ProductName=@ProductName, Quantity=@Quantity, Price=@Price
                           WHERE OrderItemId=@OrderItemId";
            return await conn.ExecuteAsync(sql, orderItem) > 0;
        }

        public async Task<bool> DeleteAsync(int orderItemId)
        {
            using var conn = CreateConnection();
            string sql = "DELETE FROM OrderItem WHERE OrderItemId=@Id";
            return await conn.ExecuteAsync(sql, new { Id = orderItemId }) > 0;
        }
    }

}
