using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
