using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BagStore.Web.Utilities
{
    public class CustomProblemDetailsExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CustomProblemDetailsExceptionHandler> _logger;

        public CustomProblemDetailsExceptionHandler(ILogger<CustomProblemDetailsExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Lỗi máy chủ nội bộ",
                Detail = "Đã xảy ra lỗi không mong muốn. Vui lòng thử lại sau.",
                Instance = httpContext.Request.Path
            };

            // Để hiển thị chi tiết lỗi trong môi trường phát triển (DEV)
            if (httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
            {
                problemDetails.Detail = $"Lỗi: {exception.Message}";
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true; // Báo hiệu rằng lỗi đã được xử lý
        }
    }
}