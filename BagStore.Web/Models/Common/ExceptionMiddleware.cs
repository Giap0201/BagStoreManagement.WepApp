using BagStore.Models.Common;
using BagStore.Web.Models.Common;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // chuyền request
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // Trả về BaseResponse chuẩn
            var error = new List<ErrorDetail> { new ErrorDetail("Exception", "Có lỗi xảy ra trên server") };
            var response = BaseResponse<string>.Error(error, "Lỗi server");
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}