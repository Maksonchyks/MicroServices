using Common.Dto;
using Orders.Domain.Enteties;

namespace Orders.Bll.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));


            return new OrderDto
            {
                OrderId = order.OrderId,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                PromotionId = order.PromotionId
            };
        }
        public static Order ToEntity(OrderDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Order
            {
                OrderId = dto.OrderId,
                CustomerName = dto.CustomerName,
                OrderDate = dto.OrderDate,
                TotalAmount = dto.TotalAmount,
                PromotionId = dto.PromotionId
            };
        }
    }
}
