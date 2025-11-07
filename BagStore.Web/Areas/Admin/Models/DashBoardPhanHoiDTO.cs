namespace BagStore.Web.Areas.Admin.Models
{
    public class DashBoardPhanHoiDTO
    {
        public ThongKeTongQuanDTO ThongKeTongQuan { get; set; }
        public BieuDoTrangThaiDonHangDTO BieuDoTrangThaiDonHang { get; set; }
        public BieuDoDoanhThuDTO BieuDoDoanhThu { get; set; }
        public List<SanPhamBanChayDTO> DanhSachSanPhamBanChay { get; set; }
        public List<DonHangGanDayDTO> DanhSachDonHangGanDay { get; set; }
    }
}