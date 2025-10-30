using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common.Dto;
using Orders.Bll.Interfaces;
using Orders.Dal.Interfaces;

namespace Orders.Bll.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public async Task AddAsync(OrderDto dto)
        {
            var entity = _mapper.Map<Orders.Domain.Enteties.Order>(dto);
            await _unitOfWork.Orders.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(OrderDto dto)
        {
            var entity = _mapper.Map<Orders.Domain.Enteties.Order>(dto);
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
