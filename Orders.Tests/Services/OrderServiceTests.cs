using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Orders.Bll.Services;
using Orders.Dal.Interfaces;
using Orders.Dal.Repositories;
using Xunit;
using FluentAssertions;
using Orders.Domain.Enteties;

namespace Orders.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            // Мокаємо метод GetAll у репозиторії Orders
            _unitOfWorkMock.Setup(u => u.Orders.GetAllAsync())
                .ReturnsAsync(new List<Order>() { new Order { OrderId = 1, CustomerName = "asd" } });

            _orderService = new OrderService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetAllOrders_ShouldReturnOrders()
        {
            var result = await _orderService.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(1);
        }
    }
}
