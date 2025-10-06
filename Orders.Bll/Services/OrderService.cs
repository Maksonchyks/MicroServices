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
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return orders.Select(OrderMapper.ToDto);
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            return order == null ? null : OrderMapper.ToDto(order);
        }

        public async Task AddAsync(OrderDto dto)
        {
            var entity = OrderMapper.ToEntity(dto);
            await _unitOfWork.Orders.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(OrderDto dto)
        {
            var entity = OrderMapper.ToEntity(dto);
            await _unitOfWork.Orders.UpdateAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Orders.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }
    }
}
