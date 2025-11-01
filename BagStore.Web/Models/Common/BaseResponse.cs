using BagStore.Web.Models.Common;

namespace BagStore.Models.Common
{
    public class BaseResponse<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<ErrorDetail> Errors { get; set; }

        public BaseResponse(string status, string message, T data, List<ErrorDetail> errors)
        {
            Status = status;
            Message = message;
            Data = data;
            Errors = errors;
        }

        public static BaseResponse<T> Success(T data, string message = "Yêu cầu được xử lý thành công")
        {
            return new BaseResponse<T>("success", message, data, null);
        }

        public static BaseResponse<T> Error(List<ErrorDetail> errors, string message = "Dữ liệu đầu vào không hợp lệ")
        {
            return new BaseResponse<T>("error", message, default, errors);
        }

        internal static object? Error(string v)
        {
            throw new NotImplementedException();
        }
    }
}