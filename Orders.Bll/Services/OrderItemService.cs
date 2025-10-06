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
    public class OrderItemService : IOrderItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllAsync()
        {
            var items = await _unitOfWork.OrderItems.GetAllAsync();
            return items.Select(OrderItemMapper.ToDto);
        }

        public async Task<OrderItemDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.OrderItems.GetByIdAsync(id);
            return item == null ? null : OrderItemMapper.ToDto(item);
        }

        public async Task AddAsync(OrderItemDto dto)
        {
            var entity = OrderItemMapper.ToEntity(dto);
            await _unitOfWork.OrderItems.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(OrderItemDto dto)
        {
            var entity = OrderItemMapper.ToEntity(dto);
            await _unitOfWork.OrderItems.UpdateAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.OrderItems.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }
    }
}
