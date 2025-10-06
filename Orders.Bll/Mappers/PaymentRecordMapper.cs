using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Orders.Domain.Enteties;

namespace Orders.Bll.Mappers
{
    public static class PaymentRecordMapper
    {
        public static PaymentRecordDto ToDto(PaymentRecord record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));

            return new PaymentRecordDto
            {
                PaymentId = record.PaymentId,
                OrderId = record.OrderId,
                Amount = record.Amount,
                PaymentDate = record.PaymentDate,
                PaymentMethod = record.PaymentMethod
            };
        }

        public static PaymentRecord ToEntity(PaymentRecordDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new PaymentRecord
            {
                PaymentId = dto.PaymentId,
                OrderId = dto.OrderId,
                Amount = dto.Amount,
                PaymentDate = dto.PaymentDate,
                PaymentMethod = dto.PaymentMethod
            };
        }
    }
}
