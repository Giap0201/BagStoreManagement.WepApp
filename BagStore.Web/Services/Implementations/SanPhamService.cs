using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Helpers;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
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

        public async Task<BaseResponse<SanPhamResponseDto>> CreateAsync(SanPhamCreateDto dto)
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

            // Upload ảnh chính
            var anhChinh = await ImageHelper.UploadSingleImageAsync(dto.AnhChinh, _env.WebRootPath);
            if (anhChinh != null)
            {
                anhChinh.LaHinhChinh = true;
                entity.AnhSanPhams.Add(anhChinh);
            }

            // Lưu vào DB
            var created = await _repo.AddAsync(entity);

            // Map sang DTO Response
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
                var dto = await MapEntityToResponse(entity); // async mapping
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

            // Lấy ảnh chính
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
    }
}