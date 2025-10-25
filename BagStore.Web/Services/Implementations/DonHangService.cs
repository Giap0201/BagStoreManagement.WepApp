using BagStore.Domain.Entities;
using BagStore.Web.AppConfig.Implementations;
using BagStore.Web.AppConfig.Interface;
using BagStore.Web.Models.DTOs.Requests;
using BagStore.Web.Models.DTOs.Response;
using BagStore.Web.Models.Entities.Enums;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;

namespace BagStore.Web.Services.Implementations
{
    public class DonHangService : IDonHangService
    {
        private readonly IDonHangRepository _donHangRepo;
        private readonly IChiTietDonHangRepository _chiTietRepo;
        private readonly IEnumMapper _mapper;
        //private readonly IChiTietSanPhamRepository _chiTietSanPhamRepo;
        //private readonly IKhachHangRepository _khachHangRepo;

        public DonHangService(
            IDonHangRepository donHangRepo,
            IChiTietDonHangRepository chiTietRepo,
            IEnumMapper mapper)
            //IChiTietSanPhamRepository chiTietSanPhamRepo,
            //IKhachHangRepository khachHangRepo)
        {
            _donHangRepo = donHangRepo;
            _chiTietRepo = chiTietRepo;
            _mapper = mapper;
            //_chiTietSanPhamRepo = chiTietSanPhamRepo;
            //_khachHangRepo = khachHangRepo;
        }

        public async Task<IEnumerable<DonHangResponse>> LayTatCaDonHangAsync()
        {
            var donHangs = await _donHangRepo.LayTatCaDonHangAsync();
            return donHangs.Select(MapToDonHangResponse);
        }

        public async Task<IEnumerable<DonHangResponse>> LayDonHangTheoUserAsync(string userId)
        {
            var entities = await _donHangRepo.LayDonHangTheoUserAsync(userId);

            if (entities == null || !entities.Any())
            {
                
                return Enumerable.Empty<DonHangResponse>();
            }

            return entities.Select(MapToDonHangResponse);
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

            var donHang = new DonHang
            {
                // Lưu MaKH test
                MaKH = maKhachHangTest,
                DiaChiGiaoHang = dto.DiaChiGiaoHang,
                PhuongThucThanhToan = dto.PhuongThucThanhToan,
                NgayDatHang = DateTime.Now,
                TongTien = dto.ChiTietDonHang.Sum(ct => ct.SoLuong * ct.GiaBan),
                TrangThai = "Chờ xử lý",
                TrangThaiThanhToan = "Chờ xác nhận",
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
            if (dto == null || string.IsNullOrWhiteSpace(dto.TrangThai))
                throw new ArgumentException("Trạng thái không hợp lệ.");

            // 1️⃣ Map string -> enum để kiểm tra logic
            TrangThaiDonHang trangThaiEnum;
            try
            {
                trangThaiEnum = _mapper.MapToEnum<TrangThaiDonHang>(dto.TrangThai.Trim());
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Trạng thái đơn hàng không hợp lệ.", ex);
            }

            // 2️⃣ Lấy đơn hàng
            var donHang = await _donHangRepo.LayTheoIdAsync(dto.MaDonHang)
                ?? throw new KeyNotFoundException("Đơn hàng không tồn tại.");

            // Map lại trạng thái cũ từ string -> enum để so sánh
            var trangThaiCu = _mapper.MapToEnum<TrangThaiDonHang>(donHang.TrangThai);

            // 3️⃣ Không cho cập nhật nếu đã Hoàn thành hoặc Đã hủy
            if (trangThaiCu == TrangThaiDonHang.HoanThanh || trangThaiCu == TrangThaiDonHang.DaHuy)
                throw new InvalidOperationException("Không thể thay đổi trạng thái của đơn hàng đã hoàn thành hoặc đã hủy.");

            // 4️⃣ Nếu trạng thái không đổi thì bỏ qua
            if (trangThaiCu == trangThaiEnum)
                return MapToDonHangResponse(donHang);

            // 5️⃣ Cập nhật trạng thái đơn hàng (enum -> string hiển thị)
            donHang.TrangThai = _mapper.MapToString(trangThaiEnum);

            // 6️⃣ Đồng bộ trạng thái thanh toán (string → enum → string)
            var trangThaiThanhToanCu = _mapper.MapToEnum<TrangThaiThanhToan>(donHang.TrangThaiThanhToan);
            var phuongThucThanhToan = _mapper.MapToEnum<TrangThaiPhuongThucThanhToan>(donHang.PhuongThucThanhToan);

            var trangThaiThanhToanMoi = trangThaiEnum switch
            {
                TrangThaiDonHang.HoanThanh => TrangThaiThanhToan.ThanhCong,
                TrangThaiDonHang.DaHuy => trangThaiThanhToanCu == TrangThaiThanhToan.ThanhCong
                                           ? TrangThaiThanhToan.DaHoanTien
                                           : TrangThaiThanhToan.ThatBai,
                TrangThaiDonHang.DangGiaoHang =>
                    phuongThucThanhToan == TrangThaiPhuongThucThanhToan.COD
                        ? TrangThaiThanhToan.ChoXacNhan
                        : TrangThaiThanhToan.ThanhCong,
                _ => TrangThaiThanhToan.ChoXacNhan
            };

            donHang.TrangThaiThanhToan = _mapper.MapToString(trangThaiThanhToanMoi);

            // 7️⃣ Lưu DB
            await _donHangRepo.CapNhatAsync(donHang);
            await _donHangRepo.LuuAsync();

            // 8️⃣ Trả về response
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
