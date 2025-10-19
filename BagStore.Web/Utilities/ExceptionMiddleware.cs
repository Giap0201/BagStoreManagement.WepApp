//using BagStore.Web.Models.DTOs;
//using BagStore.Web.Utilities;
//using System.Net;
//using System.Text.Json;

//namespace BagStore.Web.Middlewares
//{
//    public class ExceptionMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ILogger<ExceptionMiddleware> _logger;

//        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
//        {
//            _next = next;
//            _logger = logger;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            try
//            {
//                await _next(context);
//            }
//            catch (BusinessException ex)
//            {
//                _logger.LogWarning("Business exception: {Msg}", ex.Message);
//                await WriteResponseAsync(context, HttpStatusCode.BadRequest,
//                    ApiResult<string>.FailResult(ex.Message));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Unhandled exception");
//                await WriteResponseAsync(context, HttpStatusCode.InternalServerError,
//                    ApiResult<string>.FailResult("Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau."));
//            }
//        }

//        private static async Task WriteResponseAsync(HttpContext context, HttpStatusCode statusCode, object result)
//        {
//            context.Response.ContentType = "application/json";
//            context.Response.StatusCode = (int)statusCode;
//            var json = JsonSerializer.Serialize(result);
//            await context.Response.WriteAsync(json);
//        }
//    }
//}