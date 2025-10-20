using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.DTOs.SanPhams
{
    public class AnhSanPhamRequestDto
    {
        /// Mã ảnh (chỉ dùng khi cập nhật hoặc xóa).

        public int? MaAnh { get; set; }
        /// Mã sản phẩm cha mà ảnh này thuộc về.

        [Required(ErrorMessage = "Mã sản phẩm không được để trống.")]
        public int MaSP { get; set; }

        /// File ảnh được upload từ form.
        [Required(ErrorMessage = "Vui lòng chọn file ảnh.")]
        public IFormFile FileAnh { get; set; }

        /// Thứ tự hiển thị ảnh trong danh sách.
        [Range(1, int.MaxValue, ErrorMessage = "Thứ tự hiển thị phải lớn hơn 0.")]
        public int ThuTuHienThi { get; set; } = 1;

        /// Xác định ảnh này có phải ảnh chính hay không.
        public bool LaHinhChinh { get; set; } = false;
    }
}