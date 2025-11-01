using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Services;
using BagStore.Web.AppConfig.Implementations;
using BagStore.Web.AppConfig.Interface;
using BagStore.Web.Models.DTOs.Requests;
using BagStore.Web.Models.DTOs.Response;
using BagStore.Web.Models.Entities.Enums;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Services.Implementations
{
    public class DonHangService : IDonHangService
    {
        private readonly IDonHangRepository _donHangRepo;
        private readonly IChiTietDonHangRepository _chiTietRepo;
        private readonly IEnumMapper _mapper;
        private readonly BagStoreDbContext _dbContext;
        private readonly IChiTietSanPhamRepository _chiTietSanPhamRepo;
        private readonly ICartService _cartService;
        //private readonly IKhachHangRepository _khachHangRepo;

        public DonHangService(
            IDonHangRepository donHangRepo,
            IChiTietDonHangRepository chiTietRepo,
            IChiTietSanPhamRepository chiTietSanPhamRepo,
            ICartService cartService,
            IEnumMapper mapper,
            BagStoreDbContext dbContext)
            //IKhachHangRepository khachHangRepo)
        {
            _donHangRepo = donHangRepo;
            _chiTietRepo = chiTietRepo;
            _mapper = mapper;
            _cartService = cartService;
            _dbContext = dbContext;
            _chiTietSanPhamRepo = chiTietSanPhamRepo;
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

        public BagStoreDbContext Get_dbContext()
        {
            return _dbContext;
        }

        public async Task<DonHangResponse> TaoDonHangAsync(CreateDonHangRequest request, string userId)
        {
            // 1️⃣ Lấy khách hàng
            var khachHang = await _dbContext.KhachHangs
                .FirstOrDefaultAsync(x => x.ApplicationUserId == userId);
            if (khachHang == null)
                throw new KeyNotFoundException("Không tìm thấy khách hàng.");

            var maKH = khachHang.MaKH;

            // 2️⃣ Lấy danh sách sản phẩm cần thanh toán
            List<(int MaChiTietSP, int SoLuong)> sanPhamThanhToan = new();

            if (request.ChiTietDonHang != null && request.ChiTietDonHang.Any())
            {
                // 🟢 Trường hợp BUY NOW
                sanPhamThanhToan = request.ChiTietDonHang
                    .Select(x => (x.MaChiTietSanPham, x.SoLuong))
                    .ToList();
            }
            else
            {
                // 🟢 Trường hợp lấy từ GIỎ HÀNG
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                if (cart == null || !cart.Items.Any())
                    throw new InvalidOperationException("Giỏ hàng của bạn đang trống.");

                sanPhamThanhToan = cart.Items
                    .Select(i => (i.MaSP_GH, i.SoLuong))
                    .ToList();
            }

            // 3️⃣ Tạo đơn hàng
            var donHang = new DonHang
            {
                MaKH = maKH,
                NgayDatHang = DateTime.Now,
                TrangThai = "Chờ xử lý",
                TrangThaiThanhToan = "Chờ xác nhận",
                DiaChiGiaoHang = request.DiaChiGiaoHang,
                PhuongThucThanhToan = request.PhuongThucThanhToan
            };
            _dbContext.DonHangs.Add(donHang);
            await _dbContext.SaveChangesAsync();

            decimal tongTien = 0;

            // 4️⃣ Duyệt và tạo chi tiết đơn hàng
            foreach (var (maChiTietSP, soLuong) in sanPhamThanhToan)
            {
                var chiTietSP = await _chiTietSanPhamRepo.GetByIdAsync(maChiTietSP);
                if (chiTietSP == null)
                    throw new KeyNotFoundException($"Không tìm thấy sản phẩm mã {maChiTietSP}");

                if (chiTietSP.SoLuongTon < soLuong)
                    throw new InvalidOperationException($"Sản phẩm {chiTietSP.SanPham.TenSP} không đủ hàng.");

                chiTietSP.SoLuongTon -= soLuong;
                await _chiTietSanPhamRepo.UpdateAsync(chiTietSP);

                var giaBan = chiTietSP.GiaBan;
                var thanhTien = giaBan * soLuong;
                tongTien += thanhTien;

                _dbContext.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaChiTietSP = maChiTietSP,
                    SoLuong = soLuong,
                    GiaBan = giaBan
                });
            }

            // 5️⃣ Cập nhật tổng tiền
            donHang.TongTien = tongTien;
            await _dbContext.SaveChangesAsync();

            // 6️⃣ Nếu đơn hàng được lấy từ giỏ → xoá giỏ hàng
            //if (request.ChiTietDonHang == null || !request.ChiTietDonHang.Any())
            //{
            //    await _cartService.ClearCartAsync(userId);
            //}

            return MapToDonHangResponse(donHang);
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

            // Nếu hủy đơn hàng thì hoàn trả tồn kho

            if (trangThaiEnum == TrangThaiDonHang.DaHuy)
            {
                var chiTietDonHangs = await _dbContext.ChiTietDonHangs
                    .Where(ct => ct.MaDonHang == donHang.MaDonHang)
                    .ToListAsync();

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

            // 7️⃣ Lưu DB
            await _donHangRepo.CapNhatAsync(donHang);
            await _donHangRepo.LuuAsync();

            // 8️⃣ Trả về response
            return MapToDonHangResponse(donHang);
        }

        public async Task<DonHangResponse?> GetByIdAsync(int maDH)
        {
            var donHang = await _donHangRepo.GetByIdWithDetailsAsync(maDH);
            if (donHang == null)
                return null;

            // Dùng mapper tách riêng để dễ tái sử dụng
            return MapToDonHangResponse(donHang);
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
                AnhSanPham = ""
            }).ToList() ?? new List<DonHangChiTietResponse>();

            return response;
        }

    }
}
