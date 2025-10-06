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
    public class PaymentRecordService : IPaymentRecordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PaymentRecordDto>> GetAllAsync()
        {
            var payments = await _unitOfWork.PaymentRecords.GetAllAsync();
            return payments.Select(PaymentRecordMapper.ToDto);
        }

        public async Task<PaymentRecordDto?> GetByIdAsync(int id)
        {
            var payment = await _unitOfWork.PaymentRecords.GetByIdAsync(id);
            return payment == null ? null : PaymentRecordMapper.ToDto(payment);
        }

        public async Task AddAsync(PaymentRecordDto dto)
        {
            var entity = PaymentRecordMapper.ToEntity(dto);
            await _unitOfWork.PaymentRecords.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(PaymentRecordDto dto)
        {
            var entity = PaymentRecordMapper.ToEntity(dto);
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
