using BagStore.Models.Common;
using BagStore.Web.Areas.Admin.Models;
using BagStore.Web.Models.Common;
using BagStore.Web.Repositories.Interfaces;

namespace BagStore.Web.Repositories.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;

        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<BaseResponse<DashBoardPhanHoiDTO>> LayDuLieuDashboardAsync(int namHienTai)
        {
            try
            {
                var thongKeTongQuan = await _repo.LayThongKeTongQuanAsync();
                var bieuDoDoanhThu = await _repo.LayBieuDoDoanhThuAsync(namHienTai);
                var bieuDoTrangThaiDonHang = await _repo.LayBieuDoTrangThaiDonHangAsync();
                var danhSachDonHangGanDay = await _repo.LayDanhSachDonHangGanDayAsync();
                var danhSachSanPhamBanChay = await _repo.LayDanhSachSanPhamBanChayAsync();
                var dashboardData = new DashBoardPhanHoiDTO
                {
                    ThongKeTongQuan = thongKeTongQuan,
                    BieuDoDoanhThu = bieuDoDoanhThu,
                    BieuDoTrangThaiDonHang = bieuDoTrangThaiDonHang,
                    DanhSachDonHangGanDay = danhSachDonHangGanDay,
                    DanhSachSanPhamBanChay = danhSachSanPhamBanChay
                };
                return BaseResponse<DashBoardPhanHoiDTO>.Success(dashboardData, "Lấy dữ liệu dashboard thành công");
            }
            catch (Exception ex)
            {
                return BaseResponse<DashBoardPhanHoiDTO>.Error(
                   new List<ErrorDetail> { new ErrorDetail("Exception", ex.Message) },
                   "Không thể tải dữ liệu Dashboard");
            }
        }
    }
}