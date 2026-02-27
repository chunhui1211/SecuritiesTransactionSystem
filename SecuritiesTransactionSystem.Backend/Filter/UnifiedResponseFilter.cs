using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SecuritiesTransactionSystem.Entity.DTOs;

namespace SecuritiesTransactionSystem.Backend.Filter
{
    public class UnifiedResponseFilter: IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // 只針對成功回傳 (200-299) 的 ObjectResult 進行包裝
            if (context.Result is ObjectResult objectResult && objectResult.StatusCode is null or >= 200 and < 300)
            {
                var response = ApiResponse<object>.SuccessResponse(objectResult.Value!);
                context.Result = new ObjectResult(response)
                {
                    StatusCode = objectResult.StatusCode
                };
            }
        }
    }
}
