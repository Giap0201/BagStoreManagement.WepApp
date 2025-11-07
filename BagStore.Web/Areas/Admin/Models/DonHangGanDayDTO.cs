namespace BagStore.Web.Areas.Admin.Models
{
    public class DonHangGanDayDTO
    {
        public int MaDonHang { get; set; }
        public string TenKhachHang { get; set; }
        public DateTime NgayDatHang { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
    }
}