using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Orders.Dal.Interfaces;
using Orders.Dal.Repositories;

namespace Orders.Dal.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly string _connectionString;
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;

        public IPromotionRepository Promotions { get; }
        public IOrderRepository Orders { get; }
        public IOrderItemRepository OrderItems { get; }
        public IOrderStatusHistoryRepository OrderStatusHistories { get; }
        public IPaymentRecordRepository PaymentRecords { get; }

        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;

            Promotions = new PromotionRepository(_connectionString);
            Orders = new OrderRepository(_connectionString);
            OrderItems = new OrderItemRepository(_connectionString);
            OrderStatusHistories = new OrderStatusHistoryRepository(_connectionString);
            PaymentRecords = new PaymentRecordRepository(_connectionString);
        }

        public async Task BeginTransactionAsync()
        {
            _connection = new MySqlConnection(_connectionString);
            await ((MySqlConnection)_connection).OpenAsync();
            _transaction = _connection.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}
