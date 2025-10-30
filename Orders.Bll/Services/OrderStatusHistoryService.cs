using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common.Dto;
using Orders.Bll.Interfaces;
using Orders.Dal.Interfaces;

namespace Orders.Bll.Services
{
    public class OrderStatusHistoryService : IOrderStatusHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderStatusHistoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderStatusHistoryDto>> GetAllAsync()
        {
            var statuses = await _unitOfWork.OrderStatusHistories.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderStatusHistoryDto>>(statuses);
        }

        public async Task<OrderStatusHistoryDto?> GetByIdAsync(int id)
        {
            var status = await _unitOfWork.OrderStatusHistories.GetByIdAsync(id);
            return status == null ? null : _mapper.Map<OrderStatusHistoryDto>(status);
        }

        public async Task AddAsync(OrderStatusHistoryDto dto)
        {
            var entity = _mapper.Map<Orders.Domain.Enteties.OrderStatusHistory>(dto);
            await _unitOfWork.OrderStatusHistories.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(OrderStatusHistoryDto dto)
        {
            var entity = _mapper.Map<Orders.Domain.Enteties.OrderStatusHistory>(dto);
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
