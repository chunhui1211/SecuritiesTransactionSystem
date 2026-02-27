using Microsoft.Extensions.Logging;
using SecuritiesTransactionSystem.Entity.DTOs;
using SecuritiesTransactionSystem.Entity.Exceptions;
using SecuritiesTransactionSystem.Entity.Model;
using SecuritiesTransactionSystem.Entity.Validator;
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
                var validator = new CreateOrderRequestValidator();
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var error = validationResult.Errors.First().ErrorMessage;
                    _logger.LogWarning("下單參數驗證失敗: {Msg}", error);
                    throw new BusinessException(error);
                }

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

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }
    }
}
