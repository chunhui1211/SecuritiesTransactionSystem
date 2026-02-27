using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuritiesTransactionSystem.Entity.Exceptions
{
    /// <summary>
    /// 業務邏輯異常
    /// </summary>
    public class BusinessException : Exception
    {
        public int StatusCode { get; }
        public BusinessException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
