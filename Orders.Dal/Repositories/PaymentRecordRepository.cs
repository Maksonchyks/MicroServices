using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Orders.Dal.Interfaces;
using Orders.Domain.Enteties;
using System.Data;
using MySqlConnector;

namespace Orders.Dal.Repositories
{
    public class PaymentRecordRepository : BaseRepository, IPaymentRecordRepository
    {
        public PaymentRecordRepository(string connectionString) : base(connectionString) { }

        protected new IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public async Task<IEnumerable<PaymentRecord>> GetAllAsync()
        {
            using var conn = CreateConnection();
            string sql = "SELECT PaymentId, OrderId, Amount, PaymentDate, PaymentMethod FROM PaymentRecord";
            return await conn.QueryAsync<PaymentRecord>(sql);
        }

        public async Task<PaymentRecord> GetByIdAsync(int paymentId)
        {
            using var conn = CreateConnection();
            string sql = "SELECT PaymentId, OrderId, Amount, PaymentDate, PaymentMethod FROM PaymentRecord WHERE PaymentId=@Id";
            return await conn.QueryFirstOrDefaultAsync<PaymentRecord>(sql, new { Id = paymentId });
        }

        public async Task<IEnumerable<PaymentRecord>> GetByOrderIdAsync(int orderId)
        {
            using var conn = CreateConnection();
            string sql = "SELECT PaymentId, OrderId, Amount, PaymentDate, PaymentMethod FROM PaymentRecord WHERE OrderId=@OrderId";
            return await conn.QueryAsync<PaymentRecord>(sql, new { OrderId = orderId });
        }

        public async Task<int> AddAsync(PaymentRecord payment)
        {
            using var conn = CreateConnection();
            string sql = @"INSERT INTO PaymentRecord (OrderId, Amount, PaymentDate, PaymentMethod)
                           VALUES (@OrderId, @Amount, @PaymentDate, @PaymentMethod);
                           SELECT LAST_INSERT_ID();";
            return await conn.QuerySingleAsync<int>(sql, payment);
        }

        public async Task<bool> UpdateAsync(PaymentRecord payment)
        {
            using var conn = CreateConnection();
            string sql = @"UPDATE PaymentRecord 
                           SET OrderId=@OrderId, Amount=@Amount, PaymentDate=@PaymentDate, PaymentMethod=@PaymentMethod
                           WHERE PaymentId=@PaymentId";
            return await conn.ExecuteAsync(sql, payment) > 0;
        }

        public async Task<bool> DeleteAsync(int paymentId)
        {
            using var conn = CreateConnection();
            string sql = "DELETE FROM PaymentRecord WHERE PaymentId=@Id";
            return await conn.ExecuteAsync(sql, new { Id = paymentId }) > 0;
        }
    }
}
