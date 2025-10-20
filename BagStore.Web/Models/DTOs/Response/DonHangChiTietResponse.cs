namespace BagStore.Web.Models.DTOs.Response
{
    public class DonHangChiTietResponse
    {
        public int MaChiTietDonHang { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal ThanhTien { get; set; } // Tính ở backend, không dùng expression
        public string AnhSanPham { get; set; } = string.Empty; // thêm nếu cần hiển thị
    }
}
