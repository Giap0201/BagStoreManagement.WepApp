using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.DTOs
{
    public class ChatLieuDto
    {
        public int MaChatLieu { get; set; }

        [Required(ErrorMessage = "Tên chất liệu không được để trống")]
        [StringLength(100, ErrorMessage = "Tên chất liệu không được vượt quá 100 ký tự")]
        public string TenChatLieu { get; set; }

        [StringLength(250, ErrorMessage = "Mô tả không được vượt quá 250 ký tự")]
        public string MoTa { get; set; }
    }
}