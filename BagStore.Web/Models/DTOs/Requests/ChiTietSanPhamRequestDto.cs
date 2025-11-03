using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.DTOs.Requests
{
    public class ChiTietSanPhamRequestDto
    {
        public int MaSanPhan { get; set; }
        public int MaChiTietSP { get; set; } // 0 cho create

        [Required(ErrorMessage = "Vui lòng chọn kích thước.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kích thước không hợp lệ.")]
        public int MaKichThuoc { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn màu sắc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Màu sắc không hợp lệ.")]
        public int MaMauSac { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng tồn.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn phải từ 0 trở lên.")]
        public int SoLuongTon { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá bán.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 0.")]
        public decimal GiaBan { get; set; }
    }
}