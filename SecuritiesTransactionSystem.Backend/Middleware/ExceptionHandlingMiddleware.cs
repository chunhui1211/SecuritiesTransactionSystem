using SecuritiesTransactionSystem.Entity.DTOs;
using SecuritiesTransactionSystem.Entity.Exceptions;

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
                context.Response.ContentType = "application/json";

                var (statusCode, message) = ex switch
                {
                    NotFoundException nfx => (StatusCodes.Status404NotFound, nfx.Message),
                    BusinessException bx => (StatusCodes.Status400BadRequest, bx.Message),
                    _ => (StatusCodes.Status500InternalServerError, "伺服器內部錯誤")
                };

                context.Response.StatusCode = statusCode;

                if (statusCode >= 500)
                    _logger.LogError(ex, "伺服器內部錯誤");
                else
                    _logger.LogWarning("業務錯誤: {Msg}", message);

                await context.Response.WriteAsJsonAsync(new ApiResponse<object>
                {
                    Success = false,
                    Message = message
                });
            }
        }
    }
}
