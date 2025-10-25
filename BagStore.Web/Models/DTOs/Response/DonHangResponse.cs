namespace BagStore.Web.Models.DTOs.Response
{
    public class DonHangResponse
    {
        public int MaDonHang { get; set; }
        public string TenKhachHang { get; set; } = string.Empty;
        public DateTime NgayDatHang { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public string PhuongThucThanhToan { get; set; } = string.Empty;
        public string TrangThaiThanhToan { get; set; } = string.Empty;
        public string DiaChiGiaoHang { get; set; } = string.Empty;

        public List<DonHangChiTietResponse> ChiTietDonHang { get; set; } = new();
    }
}
