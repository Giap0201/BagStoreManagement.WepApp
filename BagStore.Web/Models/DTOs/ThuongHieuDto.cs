using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.DTOs
{
    public class ThuongHieuDto
    {
        public int MaThuongHieu { get; set; }

        [Required(ErrorMessage = "Tên thương hiệu là bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Tên thương hiệu không được vượt quá 100 ký tự.")]
        public string TenThuongHieu { get; set; }

        [MaxLength(50, ErrorMessage = "Quốc gia không được vượt quá 50 ký tự.")]
        public string QuocGia { get; set; }
    }
}