using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Helpers;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Models.ViewModels;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;

namespace BagStore.Web.Services.Implementations
{
    public class SanPhamService : ISanPhamService
    {
        private readonly ISanPhamRepository _repo;
        private readonly IDanhMucLoaiTuiRepository _repoLoaiTui;
        private readonly IThuongHieuRepository _repoThuongHieu;
        private readonly IChatLieuRepository _repoChatLieu;
        private readonly IWebHostEnvironment _env;

        public SanPhamService(
            ISanPhamRepository repo,
            IWebHostEnvironment env,
            IDanhMucLoaiTuiRepository repoLoaiTui,
            IThuongHieuRepository repoThuongHieu,
            IChatLieuRepository repoChatLieu)
        {
            _repo = repo;
            _env = env;
            _repoLoaiTui = repoLoaiTui;
            _repoThuongHieu = repoThuongHieu;
            _repoChatLieu = repoChatLieu;
        }

        public async Task<BaseResponse<SanPhamResponseDto>> CreateAsync(SanPhamRequestDto dto)
        {
            if (dto == null)
                return BaseResponse<SanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("Dto", "Dữ liệu không được null") },
                    "Tạo mới thất bại");

            // Kiểm tra trùng tên
            var existing = await _repo.GetByNameAsync(dto.TenSP);
            if (existing != null)
                return BaseResponse<SanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail(nameof(dto.TenSP), $"Tên sản phẩm '{dto.TenSP}' đã tồn tại") },
                    "Tạo mới thất bại");

            // Map DTO → Entity
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

            // Upload ảnh chính (code của bạn đã đúng logic)
            if (dto.AnhChinh != null)
            {
                var anhChinh = await ImageHelper.UploadSingleImageAsync(dto.AnhChinh, _env.WebRootPath);
                if (anhChinh != null)
                {
                    anhChinh.LaHinhChinh = true;
                    anhChinh.ThuTuHienThi = 1; // Gán thứ tự 1 cho ảnh đầu tiên
                    entity.AnhSanPhams.Add(anhChinh);
                }
            }

            // Lưu vào DB
            var created = await _repo.AddAsync(entity);
            var createdWithIncludes = await _repo.GetByIdAsync(created.MaSP);
            var sanPhamResponDto = MapEntityToResponse(createdWithIncludes);
            return BaseResponse<SanPhamResponseDto>.Success(sanPhamResponDto, "Tạo mới sản phẩm thành công");
        }

        public async Task<BaseResponse<SanPhamResponseDto>> GetByIdAsync(int maSanPham)
        {
            var entity = await _repo.GetByIdAsync(maSanPham);
            if (entity == null)
                return BaseResponse<SanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaSanPham", "Không tìm thấy sản phẩm") },
                    "Lấy dữ liệu thất bại");

            var sanPhamResponDto = MapEntityToResponse(entity);
            return BaseResponse<SanPhamResponseDto>.Success(sanPhamResponDto, "Lấy sản phẩm thành công");
        }

        public async Task<BaseResponse<List<SanPhamResponseDto>>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            var dtos = entities.Select(MapEntityToResponse).ToList();

            return BaseResponse<List<SanPhamResponseDto>>.Success(dtos, "Lấy danh sách sản phẩm thành công");
        }

        public async Task<BaseResponse<SanPhamResponseDto>> UpdateAsync(int maSanPham, SanPhamRequestDto dto)
        {
            if (dto == null)
                return BaseResponse<SanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("Dto", "Dữ liệu không được null") },
                    "Cập nhật thất bại");

            var entity = await _repo.GetByIdAsync(maSanPham);
            if (entity == null)
                return BaseResponse<SanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaSanPham", "Sản phẩm không tồn tại") },
                    "Cập nhật sản phẩm thất bại");

            // Kiểm tra trùng tên (ngoại trừ chính nó)
            var existing = await _repo.GetByNameAsync(dto.TenSP);
            if (existing != null && existing.MaSP != maSanPham)
                return BaseResponse<SanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail(nameof(dto.TenSP), $"Tên sản phẩm '{dto.TenSP}' đã tồn tại") },
                    "Cập nhật thất bại");

            // Map DTO → Entity
            entity.TenSP = dto.TenSP;
            entity.MoTaChiTiet = dto.MoTaChiTiet;
            entity.MetaTitle = dto.MetaTitle;
            entity.MetaDescription = dto.MetaDescription;
            entity.MaLoaiTui = dto.MaLoaiTui;
            entity.MaThuongHieu = dto.MaThuongHieu;
            entity.MaChatLieu = dto.MaChatLieu;
            entity.NgayCapNhat = DateTime.Now;

            // Nếu có ảnh mới
            if (dto.AnhChinh != null)
            {
                var anhChinh = await ImageHelper.UploadSingleImageAsync(dto.AnhChinh, _env.WebRootPath);
                if (anhChinh != null)
                {
                    anhChinh.LaHinhChinh = true;
                    int maxOrder = entity.AnhSanPhams.Any() ? entity.AnhSanPhams.Max(a => a.ThuTuHienThi) : 0;
                    anhChinh.ThuTuHienThi = maxOrder + 1; // Gán thứ tự tiếp theo

                    var oldMain = entity.AnhSanPhams.FirstOrDefault(a => a.LaHinhChinh);
                    if (oldMain != null)
                    {
                        oldMain.LaHinhChinh = false;
                    }

                    entity.AnhSanPhams.Add(anhChinh);
                }
            }
            var updated = await _repo.UpdateAsync(entity);
            var sanPhamResponDto = MapEntityToResponse(updated);

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

        private SanPhamResponseDto MapEntityToResponse(SanPham entity)
        {
            // Lấy ảnh chính từ list đã Include
            var anhChinh = entity.AnhSanPhams?.FirstOrDefault(a => a.LaHinhChinh);

            return new SanPhamResponseDto
            {
                MaSP = entity.MaSP,
                TenSP = entity.TenSP,
                MoTaChiTiet = entity.MoTaChiTiet,
                MetaTitle = entity.MetaTitle,
                MetaDescription = entity.MetaDescription,
                MaLoaiTui = entity.MaLoaiTui,
                TenLoaiTui = entity.DanhMucLoaiTui?.TenLoaiTui ?? "N/A",
                MaThuongHieu = entity.MaThuongHieu,
                TenThuongHieu = entity.ThuongHieu?.TenThuongHieu ?? "N/A",
                MaChatLieu = entity.MaChatLieu,
                TenChatLieu = entity.ChatLieu?.TenChatLieu ?? "N/A",
                NgayCapNhap = entity.NgayCapNhat,
                AnhChinh = anhChinh?.DuongDan
            };
        }

        public async Task<BaseResponse<PageResult<SanPhamResponseDto>>> GetAllPagingAsync(int page, int pageSize, string? search = null)
        {
            //Dam bao gia tri phan trang hop le
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;
            //goi repository de lay du lieu
            var pagedResult = await _repo.GetAllPagingAsync(page, pageSize, search);

            //map ket qua tu PageResult<SanPham> sang PageResult<SanPhamResponseDto>
            var dtos = pagedResult.Data.Select(MapEntityToResponse).ToList();

            //tao pageResult moi voi du lieu da map
            var pagedDtoResult = new PageResult<SanPhamResponseDto>
            {
                Data = dtos,
                Total = pagedResult.Total,
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize
            };
            return BaseResponse<PageResult<SanPhamResponseDto>>.Success(pagedDtoResult, "Lấy danh sách sản phẩm phân trang thành công");
        }
    }
}