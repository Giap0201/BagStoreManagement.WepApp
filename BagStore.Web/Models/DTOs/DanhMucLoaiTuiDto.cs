using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.DTOs
{
    public class DanhMucLoaiTuiDto
    {
        public int MaLoaiTui { get; set; }

        [Required(ErrorMessage = "Tên loại túi là bắt buộc.")]
        [StringLength(100, MinimumLength = 3,
                      ErrorMessage = "Tên loại túi phải dài từ 3 đến 100 ký tự.")]
        public string TenLoaiTui { get; set; }

        [MaxLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        public string? MoTa { get; set; }
    }
}