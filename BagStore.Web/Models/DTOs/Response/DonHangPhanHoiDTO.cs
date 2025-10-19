namespace BagStore.Web.Models.DTOs.Response
{
    public class DonHangPhanHoiDTO
    {
        public int MaDonHang { get; set; }
        public string TenKH { get; set; }
        public DateTime NgayDatHang { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public string TrangThaiThanhToan { get; set; }

        public List<DonHangChiTietPhanHoiDTO> ChiTietDonHangs { get; set; } = new();
    }
}
