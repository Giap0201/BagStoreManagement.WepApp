using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.ViewModels
{
    public class ProfileEditModel
    {
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
    }
}
