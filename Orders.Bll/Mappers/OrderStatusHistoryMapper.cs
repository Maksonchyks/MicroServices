using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Orders.Domain.Enteties;

namespace Orders.Bll.Mappers
{
    public static class OrderStatusHistoryMapper
    {
        public static OrderStatusHistoryDto ToDto(OrderStatusHistory history)
        {
            if (history == null) throw new ArgumentNullException(nameof(history));

            return new OrderStatusHistoryDto
            {
                StatusId = history.StatusId,
                OrderId = history.OrderId,
                Status = history.Status,
                ChangedAt = history.ChangedAt
            };
        }

        public static OrderStatusHistory ToEntity(OrderStatusHistoryDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new OrderStatusHistory
            {
                StatusId = dto.StatusId,
                OrderId = dto.OrderId,
                Status = dto.Status,
                ChangedAt = dto.ChangedAt
            };
        }
    }
}
