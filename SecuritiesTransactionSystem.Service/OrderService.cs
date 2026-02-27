using SecuritiesTransactionSystem.Entity.DTOs;
using SecuritiesTransactionSystem.Entity.Model;
using SecuritiesTransactionSystem.Repository.Interface;
using SecuritiesTransactionSystem.Service.Interface;

namespace SecuritiesTransactionSystem.Service
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
        {
            if (request.Quantity <= 0) throw new ArgumentException("數量必須大於 0");

            var orderEntity = new Order
            {
                Id = Guid.NewGuid(),
                Symbol = request.Symbol,
                Price = request.Price,
                Quantity = request.Quantity,
                Side = request.Side
            };

            var result = await _orderRepository.AddAsync(orderEntity);

            return new OrderResponse
            {
                OrderId = result.Id,
                Message = "下單成功",
                OrderTime = result.CreatedTime
            };
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }
    }
}
