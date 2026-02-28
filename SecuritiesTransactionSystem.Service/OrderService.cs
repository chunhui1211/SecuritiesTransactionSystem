using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SecuritiesTransactionSystem.Entity.DTOs;
using SecuritiesTransactionSystem.Entity.Enums;
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
                // 基本參數驗證
                var validator = new CreateOrderRequestValidator();
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var error = validationResult.Errors.First().ErrorMessage;
                    _logger.LogWarning("下單參數驗證失敗: {Msg}", error);
                    throw new BusinessException(error);
                }

                // 針對賣出進行庫存檢查
                if (request.Side == SideType.Sell)
                {
                    // 取得該代號的所有歷史委託單
                    var userOrders = await GetBySymbolAsync(request.Symbol);

                    // 計算目前持股庫存
                    int currentStockCount = userOrders
                        .Where(o => o.Side == SideType.Buy).Sum(o => o.Quantity) -
                        userOrders
                        .Where(o => o.Side == SideType.Sell).Sum(o => o.Quantity);

                    if (currentStockCount < request.Quantity)
                    {
                        _logger.LogWarning("下單失敗：庫存不足。目前持有: {Current}, 欲賣出: {Request}", currentStockCount, request.Quantity);
                        throw new BusinessException("持股餘額不足");
                    }
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

        private async Task<IEnumerable<Order>> GetBySymbolAsync(string symbol)
        {
            return await _orderRepository.GetBySymbolAsync(symbol);
        }
    }
}
