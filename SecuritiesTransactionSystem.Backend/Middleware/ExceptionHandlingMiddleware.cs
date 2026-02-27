using SecuritiesTransactionSystem.Backend.Controllers;
using SecuritiesTransactionSystem.Entity.DTOs;

namespace SecuritiesTransactionSystem.Backend.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, " Exception: {Message}", ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new ApiResponse<object>
                {
                    Success = false,
                    Message = "伺服器內部錯誤，請聯絡系統管理員。"
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
