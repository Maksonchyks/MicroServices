using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Orders.Domain.Enteties;

namespace Orders.Bll.Mappers
{
    public static class OrderItemMapper
    {
        public static OrderItemDto ToDto(OrderItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return new OrderItemDto
            {
                OrderItemId = item.OrderItemId,
                OrderId = item.OrderId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                Price = item.Price
            };
        }

        public static OrderItem ToEntity(OrderItemDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new OrderItem
            {
                OrderItemId = dto.OrderItemId,
                OrderId = dto.OrderId,
                ProductName = dto.ProductName,
                Quantity = dto.Quantity,
                Price = dto.Price
            };
        }
    }
}
