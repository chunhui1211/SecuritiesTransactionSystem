using SecuritiesTransactionSystem.Entity.DTOs;

namespace SecuritiesTransactionSystem.Backend.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new ApiResponse<object>
                {
                    Success = false,
                    Message = $"系統錯誤: {ex.Message}"
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
