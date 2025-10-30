using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common.Dto;
using Orders.Bll.Interfaces;
using Orders.Dal.Interfaces;

namespace Orders.Bll.Services
{
    public class PaymentRecordService : IPaymentRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentRecordService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentRecordDto>> GetAllAsync()
        {
            var payments = await _unitOfWork.PaymentRecords.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentRecordDto>>(payments);
        }

        public async Task<PaymentRecordDto?> GetByIdAsync(int id)
        {
            var payment = await _unitOfWork.PaymentRecords.GetByIdAsync(id);
            return payment == null ? null : _mapper.Map<PaymentRecordDto>(payment);
        }

        public async Task AddAsync(PaymentRecordDto dto)
        {
            var entity = _mapper.Map<Orders.Domain.Enteties.PaymentRecord>(dto);
            await _unitOfWork.PaymentRecords.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(PaymentRecordDto dto)
        {
            var entity = _mapper.Map<Orders.Domain.Enteties.PaymentRecord>(dto);
            await _unitOfWork.PaymentRecords.UpdateAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.PaymentRecords.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }
    }
}
