using BagStore.Web.Areas.Admin.Models;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        Task<ThongKeTongQuanDTO> LayThongKeTongQuanAsync();

        Task<BieuDoDoanhThuDTO> LayBieuDoDoanhThuAsync(int nam);

        Task<BieuDoTrangThaiDonHangDTO> LayBieuDoTrangThaiDonHangAsync();

        Task<List<DonHangGanDayDTO>> LayDanhSachDonHangGanDayAsync(int soLuong = 5);

        Task<List<SanPhamBanChayDTO>> LayDanhSachSanPhamBanChayAsync(int soLuong = 5);
    }
}