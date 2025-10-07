using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Orders.Dal.Interfaces;
using Orders.Domain.Enteties;
using System.Data;
using MySqlConnector;

namespace Orders.Dal.Repositories
{
    public class PromotionRepository : BaseRepository, IPromotionRepository
    {
        public PromotionRepository(string connectionString) : base(connectionString) { }

        protected new IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public async Task<Promotion> GetByIdAsync(int promotionId)
        {
            using var conn = CreateConnection();
            string sql = "SELECT PromotionId, PromoCode, DiscountPercent, ExpiryDate FROM Promotion WHERE PromotionId = @Id";
            return await conn.QueryFirstOrDefaultAsync<Promotion>(sql, new { Id = promotionId });
        }

        public async Task<IEnumerable<Promotion>> GetAllAsync()
        {
            using var conn = CreateConnection();
            string sql = "SELECT PromotionId, PromoCode, DiscountPercent, ExpiryDate FROM Promotion";
            return await conn.QueryAsync<Promotion>(sql);
        }

        public async Task<int> AddAsync(Promotion promotion)
        {
            using var conn = CreateConnection();
            string sql = @"INSERT INTO Promotion (PromoCode, DiscountPercent, ExpiryDate)
                           VALUES (@PromoCode, @DiscountPercent, @ExpiryDate);
                           SELECT LAST_INSERT_ID();";
            return await conn.QuerySingleAsync<int>(sql, promotion);
        }

        public async Task<bool> UpdateAsync(Promotion promotion)
        {
            using var conn = CreateConnection();
            string sql = @"UPDATE Promotion 
                           SET PromoCode=@PromoCode, DiscountPercent=@DiscountPercent, ExpiryDate=@ExpiryDate
                           WHERE PromotionId=@PromotionId";
            return await conn.ExecuteAsync(sql, promotion) > 0;
        }

        public async Task<bool> DeleteAsync(int promotionId)
        {
            using var conn = CreateConnection();
            string sql = "DELETE FROM Promotion WHERE PromotionId=@Id";
            return await conn.ExecuteAsync(sql, new { Id = promotionId }) > 0;
        }
    }
}
