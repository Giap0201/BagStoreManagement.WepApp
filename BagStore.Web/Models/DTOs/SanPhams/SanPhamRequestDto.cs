using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.DTOs.SanPhams
{
    public class SanPhamRequestDto
    {
        public int MaSanPham { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm.")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được vượt quá 200 ký tự.")]
        public string TenSP { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả chi tiết.")]
        public string MoTaChiTiet { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Meta title.")]
        [StringLength(200, ErrorMessage = "Meta title không được vượt quá 200 ký tự.")]
        public string MetaTitle { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập MetaDescription")]
        [StringLength(500, ErrorMessage = "Meta description không được vượt quá 500 ký tự.")]
        public string MetaDescription { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại túi.")]
        [Range(1, int.MaxValue, ErrorMessage = "Loại túi không hợp lệ.")]
        public int MaLoaiTui { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thương hiệu.")]
        [Range(1, int.MaxValue, ErrorMessage = "Thương hiệu không hợp lệ.")]
        public int MaThuongHieu { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn chất liệu.")]
        [Range(1, int.MaxValue, ErrorMessage = "Chất liệu không hợp lệ.")]
        public int MaChatLieu { get; set; }

        public IFormFile? AnhChinh { get; set; }
    }
}