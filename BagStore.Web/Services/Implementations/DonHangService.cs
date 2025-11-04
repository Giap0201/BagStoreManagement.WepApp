using BagStore.Domain.Entities;
using BagStore.Services;
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
        private readonly IChiTietDonHangRepository _chiTietDonHangRepo;
        private readonly IChiTietSanPhamRepository _chiTietSanPhamRepo;
        private readonly IKhachHangRepository _khachHangRepo;
        private readonly IEnumMapper _mapper;
        private readonly ICartService _cartService;

        public DonHangService(
            IDonHangRepository donHangRepo,
            IChiTietDonHangRepository chiTietDonHangRepo,
            IChiTietSanPhamRepository chiTietSanPhamRepo,
            IKhachHangRepository khachHangRepo,
            ICartService cartService,
            IEnumMapper mapper)
        {
            _donHangRepo = donHangRepo;
            _chiTietDonHangRepo = chiTietDonHangRepo;
            _chiTietSanPhamRepo = chiTietSanPhamRepo;
            _khachHangRepo = khachHangRepo;
            _mapper = mapper;
            _cartService = cartService;
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

        public async Task<DonHangResponse> TaoDonHangAsync(CreateDonHangRequest request, string userId)
        {
            // 1️⃣ Lấy khách hàng qua repository
            var khachHang = await _khachHangRepo.GetByApplicationUserIdAsync(userId);
            if (khachHang == null)
                throw new KeyNotFoundException("Không tìm thấy khách hàng.");

            var maKH = khachHang.MaKH;

            // 2️⃣ Lấy danh sách sản phẩm cần thanh toán
            List<(int MaChiTietSP, int SoLuong)> sanPhamThanhToan = new();

            if (request.ChiTietDonHang != null && request.ChiTietDonHang.Any())
            {
                sanPhamThanhToan = request.ChiTietDonHang
                    .Select(x => (x.MaChiTietSanPham, x.SoLuong))
                    .ToList();
            }
            else
            {
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                if (cart == null || !cart.Items.Any())
                    throw new InvalidOperationException("Giỏ hàng của bạn đang trống.");

                sanPhamThanhToan = cart.Items
                    .Select(i => (i.MaSP_GH, i.SoLuong))
                    .ToList();
            }

            // 3️⃣ Tạo đơn hàng (chưa có chi tiết)
            var donHang = new DonHang
            {
                MaKH = maKH,
                NgayDatHang = DateTime.Now,
                TrangThai = "Chờ xử lý",
                TrangThaiThanhToan = "Chờ xác nhận",
                DiaChiGiaoHang = request.DiaChiGiaoHang,
                PhuongThucThanhToan = request.PhuongThucThanhToan,
                TongTien = 0m
            };

            await _donHangRepo.AddAsync(donHang);
            await _donHangRepo.LuuAsync(); // để có MaDonHang nếu DB tạo identity

            decimal tongTien = 0;

            // 4️⃣ Duyệt và tạo chi tiết đơn hàng
            foreach (var (maChiTietSP, soLuong) in sanPhamThanhToan)
            {
                var chiTietSP = await _chiTietSanPhamRepo.GetByIdAsync(maChiTietSP);
                if (chiTietSP == null)
                    throw new KeyNotFoundException($"Không tìm thấy sản phẩm mã {maChiTietSP}");

                if (chiTietSP.SoLuongTon < soLuong)
                    throw new InvalidOperationException($"Sản phẩm {chiTietSP.SanPham?.TenSP ?? maChiTietSP.ToString()} không đủ hàng.");

                // giảm tồn kho và cập nhật qua repository
                chiTietSP.SoLuongTon -= soLuong;
                await _chiTietSanPhamRepo.UpdateAsync(chiTietSP);

                var giaBan = chiTietSP.GiaBan;
                var thanhTien = giaBan * soLuong;
                tongTien += thanhTien;

                var chiTietDonHang = new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaChiTietSP = maChiTietSP,
                    SoLuong = soLuong,
                    GiaBan = giaBan
                };

                await _chiTietDonHangRepo.AddAsync(chiTietDonHang);
            }

            // 5️⃣ Cập nhật tổng tiền
            donHang.TongTien = tongTien;
            await _donHangRepo.UpdateAsync(donHang);
            await _donHangRepo.LuuAsync();

            // 6️⃣ Xóa giỏ hàng tương ứng
            var userCart = await _cartService.GetCartByUserIdAsync(userId);
            if (userCart != null && userCart.Items.Any())
            {
                if (request.ChiTietDonHang != null && request.ChiTietDonHang.Any())
                {
                    var idsMua = sanPhamThanhToan.Select(x => x.MaChiTietSP).ToHashSet();
                    var itemsToRemove = userCart.Items
                        .Where(i => idsMua.Contains(i.MaChiTietSP))
                        .ToList();

                    foreach (var item in itemsToRemove)
                    {
                        await _cartService.RemoveCartItemAsync(userId, item.MaChiTietSP);
                    }
                }
                else
                {
                    await _cartService.ClearCartAsync(userId);
                }
            }

            // trả về response (nếu cần load chi tiết đầy đủ, có thể gọi GetByIdWithDetailsAsync)
            var donHangWithDetails = await _donHangRepo.GetByIdWithDetailsAsync(donHang.MaDonHang);
            return MapToDonHangResponse(donHangWithDetails ?? donHang);
        }

        public async Task<DonHangResponse> CapNhatTrangThaiAsync(UpdateDonHangStatusRequest dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TrangThai))
                throw new ArgumentException("Trạng thái không hợp lệ.");

            TrangThaiDonHang trangThaiEnum;
            try
            {
                trangThaiEnum = _mapper.MapToEnum<TrangThaiDonHang>(dto.TrangThai.Trim());
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Trạng thái đơn hàng không hợp lệ.", ex);
            }

            var donHang = await _donHangRepo.LayTheoIdAsync(dto.MaDonHang)
                ?? throw new KeyNotFoundException("Đơn hàng không tồn tại.");

            var trangThaiCu = _mapper.MapToEnum<TrangThaiDonHang>(donHang.TrangThai);

            if (trangThaiCu == TrangThaiDonHang.HoanThanh || trangThaiCu == TrangThaiDonHang.DaHuy)
                throw new InvalidOperationException("Không thể thay đổi trạng thái của đơn hàng đã hoàn thành hoặc đã hủy.");

            if (trangThaiCu == trangThaiEnum)
                return MapToDonHangResponse(donHang);

            donHang.TrangThai = _mapper.MapToString(trangThaiEnum);

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

            // Nếu hủy đơn hàng -> hoàn trả tồn kho
            if (trangThaiEnum == TrangThaiDonHang.DaHuy)
            {
                // Lấy chi tiết đơn hàng qua repository
                var chiTietDonHangs = await _chiTietDonHangRepo.LayTheoDonHangAsync(donHang.MaDonHang);
                foreach (var ct in chiTietDonHangs)
                {
                    var chiTietSP = await _chiTietSanPhamRepo.GetByIdAsync(ct.MaChiTietSP);
                    if (chiTietSP != null)
                    {
                        chiTietSP.SoLuongTon += ct.SoLuong;
                        await _chiTietSanPhamRepo.UpdateAsync(chiTietSP);
                    }
                }
            }

            await _donHangRepo.UpdateAsync(donHang);
            await _donHangRepo.LuuAsync();

            var donHangWithDetails = await _donHangRepo.GetByIdWithDetailsAsync(donHang.MaDonHang);
            return MapToDonHangResponse(donHangWithDetails ?? donHang);
        }

        public async Task<DonHangResponse?> GetByIdAsync(int maDH)
        {
            var donHang = await _donHangRepo.GetByIdWithDetailsAsync(maDH);
            if (donHang == null)
                return null;

            return MapToDonHangResponse(donHang);
        }

        public async Task<bool> XoaDonHangAsync(int maDonHang)
        {
            // 1️⃣ Tìm đơn hàng theo ID (kèm chi tiết)
            var donHang = await _donHangRepo.GetByIdWithDetailsAsync(maDonHang);
            if (donHang == null)
                return false; // không tìm thấy

            // 2️⃣ Hoàn trả tồn kho (nếu có chi tiết)
            if (donHang.ChiTietDonHangs != null && donHang.ChiTietDonHangs.Any())
            {
                foreach (var ct in donHang.ChiTietDonHangs)
                {
                    var chiTietSP = await _chiTietSanPhamRepo.GetByIdAsync(ct.MaChiTietSP);
                    if (chiTietSP != null)
                    {
                        chiTietSP.SoLuongTon += ct.SoLuong;
                        await _chiTietSanPhamRepo.UpdateAsync(chiTietSP);
                    }
                }
            }

            // 3️⃣ Xóa toàn bộ chi tiết đơn hàng trước (tránh lỗi FK)
            if (donHang.ChiTietDonHangs != null && donHang.ChiTietDonHangs.Any())
            {
                foreach (var ct in donHang.ChiTietDonHangs)
                {
                    await _chiTietDonHangRepo.XoaAsync(ct);
                }
            }

            // 4️⃣ Xóa đơn hàng chính
            await _donHangRepo.XoaAsync(donHang.MaDonHang);

            // 5️⃣ Lưu thay đổi
            await _donHangRepo.LuuAsync();

            return true;
        }

        private static DonHangResponse MapToDonHangResponse(DonHang d)
        {
            var response = new DonHangResponse
            {
                MaDonHang = d.MaDonHang,
                TenKhachHang = d.KhachHang?.TenKH ?? "Không rõ",
                SoDienThoai = d.KhachHang?.SoDienThoai ?? "",
                NgayDatHang = d.NgayDatHang,
                TongTien = d.TongTien,
                TrangThai = string.IsNullOrEmpty(d.TrangThai) ? "Chờ xử lý" : d.TrangThai,
                PhuongThucThanhToan = string.IsNullOrEmpty(d.PhuongThucThanhToan) ? "COD" : d.PhuongThucThanhToan,
                TrangThaiThanhToan = string.IsNullOrEmpty(d.TrangThaiThanhToan) ? "Chờ xác nhận" : d.TrangThaiThanhToan,
                DiaChiGiaoHang = d.DiaChiGiaoHang ?? d.KhachHang?.DiaChiMacDinh ?? "Không có địa chỉ"
            };

            response.ChiTietDonHang = d.ChiTietDonHangs?.Select(ct => new DonHangChiTietResponse
            {
                MaChiTietDonHang = ct.MaChiTietDH,
                MaChiTietSP = ct.MaChiTietSP,
                TenSanPham = ct.ChiTietSanPham?.SanPham?.TenSP ?? "Không rõ",
                KichThuoc = ct.ChiTietSanPham?.KichThuoc?.TenKichThuoc ?? "-",
                MauSac = ct.ChiTietSanPham?.MauSac?.TenMauSac ?? "-",
                SoLuong = ct.SoLuong,
                GiaBan = ct.GiaBan,
                ThanhTien = ct.GiaBan * ct.SoLuong,
                AnhSanPham = ct.ChiTietSanPham?.SanPham?.AnhSanPhams?
                    .FirstOrDefault(a => a.LaHinhChinh)?.DuongDan ?? ""
            }).ToList() ?? new List<DonHangChiTietResponse>();

            return response;
        }
    }
}
