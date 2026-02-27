using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuritiesTransactionSystem.Entity.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Payload { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public static ApiResponse<T> SuccessResponse(T data, string message = "操作成功")
            => new() { Success = true, Payload = data, Message = message };
    }
}
