using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SecuritiesTransactionSystem.Entity.DTOs;
using SecuritiesTransactionSystem.Entity.Enums;
using SecuritiesTransactionSystem.Entity.Exceptions;
using SecuritiesTransactionSystem.Repository.Interface;
using SecuritiesTransactionSystem.Service;

namespace SecuritiesTransactionSystem.Test
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockRepo;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockRepo = new Mock<IOrderRepository>();
            _orderService = new OrderService(_mockRepo.Object, new NullLogger<OrderService>());
        }

        [Fact]
        public async Task CreateOrder_SellWithNoStock_ShouldThrowBusinessException()
        {
            var sellRequest = new CreateOrderRequest
            {
                Symbol = "2330",
                Side = SideType.Sell,
                Quantity = 10,
                Price = 600
            };

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                _orderService.CreateOrderAsync(sellRequest));

            Assert.Contains("持股餘額不足", exception.Message);
        }
    }
}