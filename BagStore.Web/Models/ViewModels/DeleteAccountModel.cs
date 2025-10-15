using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.ViewModels
{
    public class DeleteAccountModel
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string CurrentPassword { get; set; }
    }
}
