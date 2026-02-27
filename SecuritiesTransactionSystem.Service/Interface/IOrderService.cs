using SecuritiesTransactionSystem.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuritiesTransactionSystem.Service.Interface
{
    public interface IOrderService
    {
        /// <summary>
        /// 建立委託單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
    }
}
