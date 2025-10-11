using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.DTOs
{
    public class MauSacDto
    {
        public int MaMauSac { get; set; }

        [Required(ErrorMessage = "Tên màu là bắt buộc.")]
        public string TenMauSac { get; set; }
    }
}