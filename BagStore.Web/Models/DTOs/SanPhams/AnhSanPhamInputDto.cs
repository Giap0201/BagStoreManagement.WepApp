using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.DTOs.SanPhams
{
    // Input DTO cho ảnh (chỉ dùng cho update ảnh cũ)
    public class AnhSanPhamInputDto
    {
        public int MaAnh { get; set; } // Cho ảnh cũ
        public string DuongDan { get; set; } // Cho ảnh cũ

        [Range(1, int.MaxValue, ErrorMessage = "Thứ tự hiển thị phải lớn hơn 0.")]
        public int ThuTuHienThi { get; set; }

        public bool LaHinhChinh { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}