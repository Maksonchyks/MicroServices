using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Orders.Bll.Interfaces;
using Orders.Bll.Mappers;
using Orders.Dal.Interfaces;

namespace Orders.Bll.Services
{
    public class OrderStatusHistoryService : IOrderStatusHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderStatusHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrderStatusHistoryDto>> GetAllAsync()
        {
            var statuses = await _unitOfWork.OrderStatusHistories.GetAllAsync();
            return statuses.Select(OrderStatusHistoryMapper.ToDto);
        }

        public async Task<OrderStatusHistoryDto?> GetByIdAsync(int id)
        {
            var status = await _unitOfWork.OrderStatusHistories.GetByIdAsync(id);
            return status == null ? null : OrderStatusHistoryMapper.ToDto(status);
        }

        public async Task AddAsync(OrderStatusHistoryDto dto)
        {
            var entity = OrderStatusHistoryMapper.ToEntity(dto);
            await _unitOfWork.OrderStatusHistories.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(OrderStatusHistoryDto dto)
        {
            var entity = OrderStatusHistoryMapper.ToEntity(dto);
            await _unitOfWork.OrderStatusHistories.UpdateAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.OrderStatusHistories.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }
    }
}
