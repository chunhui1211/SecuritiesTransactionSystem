using Microsoft.Extensions.Logging;
using SecuritiesTransactionSystem.Entity.DTOs;
using SecuritiesTransactionSystem.Entity.Model;
using SecuritiesTransactionSystem.Repository.Interface;
using SecuritiesTransactionSystem.Service.Interface;

namespace SecuritiesTransactionSystem.Service
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
        {
            _logger.LogInformation("開始處理下單委託: {Symbol}, 數量: {Qty}", request.Symbol, request.Quantity);

            try
            {
                ValidateOrderRequest(request);

                var orderEntity = new Order
                {
                    Id = Guid.NewGuid(),
                    Symbol = request.Symbol,
                    Price = request.Price,
                    Quantity = request.Quantity,
                    Side = request.Side
                };

                var result = await _orderRepository.AddAsync(orderEntity);

                _logger.LogInformation("下單成功，委託單號: {OrderId}", result.Id);

                return new OrderResponse
                {
                    OrderId = result.Id,
                    Message = "下單成功",
                    OrderTime = result.CreatedTime
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "執行下單時發生未預期錯誤。Symbol: {Symbol}", request.Symbol);
                throw;
            }
        }

        private void ValidateOrderRequest(CreateOrderRequest request)
        {
            if (request.Quantity <= 0)
            {
                _logger.LogWarning("下單失敗：數量無效 ({Quantity})", request.Quantity);
                throw new ArgumentException("數量必須大於 0");
            }

            if (request.Price <= 0)
            {
                _logger.LogWarning("下單失敗：價格無效 ({Price})", request.Price);
                throw new ArgumentException("價格必須大於 0");
            }
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }
    }
}
