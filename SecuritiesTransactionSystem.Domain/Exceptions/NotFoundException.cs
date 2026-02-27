using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuritiesTransactionSystem.Entity.Exceptions
{
    /// <summary>
    /// 找不到資源異常
    /// </summary>
    public class NotFoundException :BusinessException
    {
        public NotFoundException(string message) : base(message, 404) { }
    }
}
