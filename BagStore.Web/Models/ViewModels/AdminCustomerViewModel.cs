using System;
using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.ViewModels
{
    public class AdminCustomerViewModel
    {
        //public string Id { get; set; } = string.Empty;
        public string? Id { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "Email là bắt buộc")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        // chỉ dùng khi thêm mới
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    //public class ResetPasswordViewModel
    //{
    //    public string Id { get; set; } = string.Empty;

    //    [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
    //    [DataType(DataType.Password)]
    //    public string NewPassword { get; set; } = string.Empty;
    //}
}
