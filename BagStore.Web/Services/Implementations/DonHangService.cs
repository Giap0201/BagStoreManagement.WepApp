using BagStore.Web.Models.DTOs.Request;
using BagStore.Web.Models.DTOs.Response;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;
using BagStore.Web.Models.Entities.Enums;
using BagStore.Domain.Entities;

namespace BagStore.Web.Services.Implementations
{
    public class DonHangService : IDonHangService
    {
        private readonly IDonHangRepository _donHangRepo;
        private readonly IChiTietDonHangRepository _chiTietRepo;
        //private readonly IChiTietSanPhamRepository _chiTietSanPhamRepo;
        //private readonly IKhachHangRepository _khachHangRepo;

        public DonHangService(
            IDonHangRepository donHangRepo,
            IChiTietDonHangRepository chiTietRepo)
            //IChiTietSanPhamRepository chiTietSanPhamRepo,
            //IKhachHangRepository khachHangRepo)
        {
            _donHangRepo = donHangRepo;
            _chiTietRepo = chiTietRepo;
            //_chiTietSanPhamRepo = chiTietSanPhamRepo;
            //_khachHangRepo = khachHangRepo;
        }

        public async Task<IEnumerable<DonHangResponse>> LayTatCaDonHangAsync()
        {
            var donHangs = await _donHangRepo.LayTatCaDonHangAsync();
            return donHangs.Select(MapToDonHangResponse);
        }

        public async Task<IEnumerable<DonHangResponse>> LayDonHangTheoKhachHangAsync(int maKhachHang)
        {
            var donHangs = await _donHangRepo.LayDonHangTheoKhachHangAsync(maKhachHang);
            return donHangs.Select(MapToDonHangResponse);
        }

        public async Task<DonHangResponse> TaoDonHangAsync(CreateDonHangRequest dto)
        {
            if (dto == null || dto.ChiTietDonHang == null || dto.ChiTietDonHang.Count == 0)
                throw new ArgumentException("Dữ liệu đơn hàng không hợp lệ.");

            // ---------------------------
            // TẠM THỜI: test cứng MaKH = 2
            // ---------------------------
            int maKhachHangTest = 2;

            // Nếu muốn khôi phục lấy MaKH từ DTO (hoặc từ auth/token), bỏ comment dòng dưới
            // var khachHang = await _khachHangRepo.LayTheoIdAsync(dto.MaKhachHang)
            //     ?? throw new ArgumentException($"Khách hàng {dto.MaKhachHang} không tồn tại.");

            // Thay bằng lookup khách hàng cho MaKH = 2 để kiểm tra phản hồi
            var khachHang = await _donHangRepo.LayTheoIdAsync(maKhachHangTest)
                ?? throw new ArgumentException($"Khách hàng {maKhachHangTest} không tồn tại (test cứng).");

            var donHang = new DonHang
            {
                // Lưu MaKH test
                MaKH = maKhachHangTest,
                DiaChiGiaoHang = dto.DiaChiGiaoHang,
                PhuongThucThanhToan = dto.PhuongThucThanhToan,
                NgayDatHang = DateTime.Now,
                TongTien = dto.ChiTietDonHang.Sum(ct => ct.SoLuong * ct.GiaBan),
                TrangThai = "Chờ xử lý",
                TrangThaiThanhToan = "Chưa thanh toán",
                PhiGiaoHang = 0
            };

            // Thêm đơn vào repo
            await _donHangRepo.ThemAsync(donHang);
            await _donHangRepo.LuuAsync();

            // Xử lý chi tiết đơn
            foreach (var ct in dto.ChiTietDonHang)
            {
                // tạo chi tiết đơn
                var chiTietDonHang = new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaChiTietSP = ct.MaChiTietSanPham,
                    SoLuong = ct.SoLuong,
                    GiaBan = ct.GiaBan
                };

                await _chiTietRepo.ThemAsync(chiTietDonHang);
            }

            // Lưu lần cuối để commit chi tiết và cập nhật tồn kho
            await _donHangRepo.LuuAsync();

            // Lấy lại đơn (đã include chi tiết, sản phẩm, khách hàng trong repository)
            var updated = await _donHangRepo.LayTheoIdAsync(donHang.MaDonHang)
                ?? throw new Exception("Không thể load lại đơn hàng.");

            return MapToDonHangResponse(updated);
        }

        public async Task<DonHangResponse> CapNhatTrangThaiAsync(UpdateDonHangStatusRequest dto)
        {
            var mapTrangThai = new Dictionary<string, TrangThaiDonHang>(StringComparer.OrdinalIgnoreCase)
            {
                { "Chờ xử lý", TrangThaiDonHang.ChoXuLy },
                { "Đang giao hàng", TrangThaiDonHang.DangGiaoHang },
                { "Hoàn thành", TrangThaiDonHang.HoanThanh },
                { "Đã huỷ", TrangThaiDonHang.DaHuy }
            };

            if (!mapTrangThai.ContainsKey(dto.TrangThai))
                throw new ArgumentException("Trạng thái đơn hàng không hợp lệ.");

            var donHang = await _donHangRepo.LayTheoIdAsync(dto.MaDonHang)
                ?? throw new KeyNotFoundException("Đơn hàng không tồn tại.");

            donHang.TrangThai = dto.TrangThai;
            await _donHangRepo.CapNhatAsync(donHang);
            await _donHangRepo.LuuAsync();

            return MapToDonHangResponse(donHang);
        }

        private static DonHangResponse MapToDonHangResponse(DonHang d)
        {
            return new DonHangResponse
            {
                MaDonHang = d.MaDonHang,
                TenKhachHang = d.KhachHang?.TenKH ?? "Khách",
                NgayDatHang = d.NgayDatHang,
                TongTien = d.TongTien,
                TrangThai = d.TrangThai,
                PhuongThucThanhToan = d.PhuongThucThanhToan,
                TrangThaiThanhToan = d.TrangThaiThanhToan,
                DiaChiGiaoHang = d.DiaChiGiaoHang,
                ChiTietDonHang = d.ChiTietDonHangs?.Select(ct => new DonHangChiTietResponse
                {
                    MaChiTietDonHang = ct.MaChiTietDH,
                    TenSanPham = ct.ChiTietSanPham?.SanPham?.TenSP ?? "Sản phẩm",
                    SoLuong = ct.SoLuong,
                    GiaBan = ct.GiaBan,
                    ThanhTien = ct.SoLuong * ct.GiaBan,
                    AnhSanPham = ""
                }).ToList() ?? new List<DonHangChiTietResponse>()
            };
        }
    }
}
