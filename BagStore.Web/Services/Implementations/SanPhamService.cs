using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;
using BagStore.Web.Utilities; // ✅ thêm using này
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Services.Implementations
{
    public class SanPhamService : ISanPhamService
    {
        private readonly ISanPhamRepository _repo;
        private readonly IDanhMucLoaiTuiRepository _repoLoaiTui;
        private readonly IThuongHieuRepository _repoThuongHieu;
        private readonly IChatLieuRepository _repoChatLieu;
        private readonly BagStoreDbContext _context;
        private readonly FileUploadService _fileUploadService; // ✅ thay vì IWebHostEnvironment

        public SanPhamService(
            BagStoreDbContext context,
            ISanPhamRepository repo,
            FileUploadService fileUploadService,
            IDanhMucLoaiTuiRepository repoLoaiTui,
            IThuongHieuRepository repoThuongHieu,
            IChatLieuRepository repoChatLieu)
        {
            _context = context;
            _repo = repo;
            _fileUploadService = fileUploadService;
            _repoLoaiTui = repoLoaiTui;
            _repoThuongHieu = repoThuongHieu;
            _repoChatLieu = repoChatLieu;
        }

        public async Task<BaseResponse<SanPhamResponseDto>> CreateAsync(SanPhamCreateDto dto)
        {
            if (dto == null)
                return BaseResponse<SanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("Dto", "Dữ liệu không được null") },
                    "Tạo mới thất bại");

            var existing = await _repo.GetByNameAsync(dto.TenSP);
            if (existing != null)
                return BaseResponse<SanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail(nameof(dto.TenSP), $"Tên sản phẩm '{dto.TenSP}' đã tồn tại") },
                    "Tạo mới thất bại");

            var entity = new SanPham
            {
                TenSP = dto.TenSP,
                MoTaChiTiet = dto.MoTaChiTiet,
                MetaTitle = dto.MetaTitle,
                MetaDescription = dto.MetaDescription,
                MaLoaiTui = dto.MaLoaiTui,
                MaChatLieu = dto.MaChatLieu,
                MaThuongHieu = dto.MaThuongHieu,
                NgayCapNhat = DateTime.Now,
                AnhSanPhams = new List<AnhSanPham>()
            };

            // ✅ Upload ảnh chính (nếu có)
            if (dto.AnhChinh != null)
            {
                var duongDan = await _fileUploadService.UploadImageAsync(dto.AnhChinh, "sanpham");
                entity.AnhSanPhams.Add(new AnhSanPham
                {
                    DuongDan = duongDan,
                    LaHinhChinh = true
                });
            }

            var created = await _repo.AddAsync(entity);
            var sanPhamResponDto = await MapEntityToResponse(created);
            return BaseResponse<SanPhamResponseDto>.Success(sanPhamResponDto, "Tạo mới sản phẩm thành công");
        }

        public async Task<BaseResponse<SanPhamResponseDto>> GetByIdAsync(int maSanPham)
        {
            var entity = await _repo.GetByIdAsync(maSanPham);
            if (entity == null)
                return BaseResponse<SanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaSanPham", "Không tìm thấy sản phẩm") },
                    "Lấy dữ liệu thất bại");

            var sanPhamResponDto = await MapEntityToResponse(entity);
            return BaseResponse<SanPhamResponseDto>.Success(sanPhamResponDto, "Lấy sản phẩm thành công");
        }

        public async Task<BaseResponse<List<SanPhamResponseDto>>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            var dtos = new List<SanPhamResponseDto>();

            foreach (var entity in entities)
            {
                var dto = await MapEntityToResponse(entity);
                dtos.Add(dto);
            }

            return BaseResponse<List<SanPhamResponseDto>>.Success(dtos, "Lấy danh sách sản phẩm thành công");
        }

        public async Task<BaseResponse<SanPhamResponseDto>> UpdateAsync(int maSanPham, SanPhamUpdateDto dto)
        {
            var entity = await _repo.GetByIdAsync(maSanPham);
            if (entity == null)
                return BaseResponse<SanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaSanPham", "Sản phẩm không tồn tại") },
                    "Cập nhật sản phẩm thất bại");

            entity.TenSP = dto.TenSanPham;
            entity.MoTaChiTiet = dto.MoTaChiTiet;
            entity.MetaTitle = dto.MetaTitle;
            entity.MetaDescription = dto.MetaDescription;
            entity.MaLoaiTui = dto.MaLoaiTui;
            entity.MaThuongHieu = dto.MaThuongHieu;
            entity.MaChatLieu = dto.MaChatLieu;
            entity.NgayCapNhat = DateTime.Now;

            // ✅ Nếu người dùng upload ảnh mới → thay ảnh chính
            if (dto.AnhChinh != null)
            {
                var duongDan = await _fileUploadService.UploadImageAsync(dto.AnhChinh, "sanpham");
                var anhChinhCu = entity.AnhSanPhams.FirstOrDefault(a => a.LaHinhChinh);
                if (anhChinhCu != null)
                    entity.AnhSanPhams.Remove(anhChinhCu);

                entity.AnhSanPhams.Add(new AnhSanPham
                {
                    DuongDan = duongDan,
                    LaHinhChinh = true
                });
            }

            var updated = await _repo.UpdateAsync(entity);
            var sanPhamResponDto = await MapEntityToResponse(updated);
            return BaseResponse<SanPhamResponseDto>.Success(sanPhamResponDto, "Cập nhật sản phẩm thành công");
        }

        public async Task<BaseResponse<bool>> DeleteAsync(int maSanPham)
        {
            var entity = await _repo.GetByIdAsync(maSanPham);
            if (entity == null)
                return BaseResponse<bool>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaSanPham", "Sản phẩm không tồn tại") },
                    "Xóa sản phẩm thất bại");

            var success = await _repo.DeleteAsync(maSanPham);
            return BaseResponse<bool>.Success(success, success ? "Xóa sản phẩm thành công" : "Xóa thất bại");
        }

        private async Task<SanPhamResponseDto> MapEntityToResponse(SanPham entity)
        {
            var danhMucLoaiTui = await _repoLoaiTui.GetByIdAsync(entity.MaLoaiTui);
            var thuongHieu = await _repoThuongHieu.GetByIdAsync(entity.MaThuongHieu);
            var chatLieu = await _repoChatLieu.GetByIdAsync(entity.MaChatLieu);
            var anhChinh = entity.AnhSanPhams?.FirstOrDefault(a => a.LaHinhChinh);

            return new SanPhamResponseDto
            {
                MaSP = entity.MaSP,
                TenSP = entity.TenSP,
                MoTaChiTiet = entity.MoTaChiTiet,
                MetaTitle = entity.MetaTitle,
                MetaDescription = entity.MetaDescription,
                TenLoaiTui = danhMucLoaiTui?.TenLoaiTui ?? "N/A",
                TenThuongHieu = thuongHieu?.TenThuongHieu ?? "N/A",
                TenChatLieu = chatLieu?.TenChatLieu ?? "N/A",
                NgayCapNhap = entity.NgayCapNhat,
                AnhChinh = anhChinh?.DuongDan
            };
        }

        public async Task<BaseResponse<PagedResult<SanPhamResponseDto>>> GetAllPagedAsync(
    int page, int pageSize, string? keyword,
    int? maLoaiTui = null, int? maThuongHieu = null, int? maChatLieu = null)
        {
            // ✅ Base query
            var query = _context.SanPhams
                .Include(sp => sp.DanhMucLoaiTui)
                .Include(sp => sp.ThuongHieu)
                .Include(sp => sp.ChatLieu)
                .Include(sp => sp.AnhSanPhams)
                .Include(sp => sp.ChiTietSanPhams) // 🔹 Thêm Include này để lấy MaChiTietSP
                .AsQueryable();

            // ✅ Bộ lọc
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.TenSP.Contains(keyword));

            if (maLoaiTui.HasValue)
                query = query.Where(x => x.MaLoaiTui == maLoaiTui.Value);

            if (maThuongHieu.HasValue)
                query = query.Where(x => x.MaThuongHieu == maThuongHieu.Value);

            if (maChatLieu.HasValue)
                query = query.Where(x => x.MaChatLieu == maChatLieu.Value);

            // ✅ Phân trang
            var totalItems = await query.CountAsync();
            var skip = (page - 1) * pageSize;

            // ✅ Lấy danh sách sản phẩm
            var items = await query
                .OrderByDescending(x => x.NgayCapNhat)
                .Skip(skip)
                .Take(pageSize)
                .Select(sp => new SanPhamResponseDto
                {
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
                    MoTaChiTiet = sp.MoTaChiTiet,
                    MetaTitle = sp.MetaTitle,
                    MetaDescription = sp.MetaDescription,

                    MaLoaiTui = sp.MaLoaiTui,
                    TenLoaiTui = sp.DanhMucLoaiTui != null ? sp.DanhMucLoaiTui.TenLoaiTui : "Không rõ",

                    MaThuongHieu = sp.MaThuongHieu,
                    TenThuongHieu = sp.ThuongHieu != null ? sp.ThuongHieu.TenThuongHieu : "Không rõ",

                    MaChatLieu = sp.MaChatLieu,
                    TenChatLieu = sp.ChatLieu != null ? sp.ChatLieu.TenChatLieu : "Không rõ",

                    // 🔹 Lấy giá bán nhỏ nhất của các biến thể (nếu có)
                    GiaBan = sp.ChiTietSanPhams.Any()
                        ? sp.ChiTietSanPhams.Min(ct => ct.GiaBan)
                        : null,

                    // 🔹 Lấy ID của biến thể đầu tiên (để mở trang chi tiết)
                    MaChiTietSP = sp.ChiTietSanPhams
                        .Select(ct => ct.MaChiTietSP)
                        .FirstOrDefault(),

                    // 🔹 Ảnh chính
                    AnhChinh = sp.AnhSanPhams
                        .Where(a => a.LaHinhChinh)
                        .Select(a => a.DuongDan)
                        .FirstOrDefault()
                        ?? "/uploads/no-image.png",

                    NgayCapNhap = sp.NgayCapNhat
                })
                .ToListAsync();

            // ✅ Kết quả phân trang
            var result = new PagedResult<SanPhamResponseDto>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };

            return BaseResponse<PagedResult<SanPhamResponseDto>>.Success(result, "Lấy danh sách sản phẩm thành công");
        }

    }
}
