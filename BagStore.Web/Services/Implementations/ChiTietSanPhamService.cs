using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;

namespace BagStore.Web.Services.Implementations
{
    public class ChiTietSanPhamService : IChiTietSanPhamService
    {
        private readonly IChiTietSanPhamRepository _repo;
        private readonly IKichThuocRepository _repoKichThuoc;
        private readonly IMauSacRepository _repoMauSac;
        private readonly ISanPhamRepository _repoSanPham;

        public ChiTietSanPhamService(
            IChiTietSanPhamRepository repo,
            IKichThuocRepository repoKichThuoc,
            IMauSacRepository repoMauSac,
            ISanPhamRepository repoSanPham)
        {
            _repo = repo;
            _repoKichThuoc = repoKichThuoc;
            _repoMauSac = repoMauSac;
            _repoSanPham = repoSanPham;
        }

        /// Tạo mới chi tiết sản phẩm

        public async Task<BaseResponse<ChiTietSanPhamResponseDto>> CreateAsync(ChiTietSanPhamRequestDto dto)
        {
            if (dto == null)
                return BaseResponse<ChiTietSanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("Dto", "Dữ liệu không được null") },
                    "Tạo mới thất bại");

            // Map DTO -> Entity
            var entity = new ChiTietSanPham
            {
                MaSP = dto.MaSanPhan,
                MaKichThuoc = dto.MaKichThuoc,
                MaMauSac = dto.MaMauSac,
                SoLuongTon = dto.SoLuongTon,
                GiaBan = dto.GiaBan,
                NgayTao = DateTime.Now
            };

            var created = await _repo.AddAsync(entity);
            var responseDto = await MapEntityToResponse(created);

            return BaseResponse<ChiTietSanPhamResponseDto>.Success(responseDto, "Tạo mới chi tiết sản phẩm thành công");
        }

        /// Cập nhật chi tiết sản phẩm

        public async Task<BaseResponse<ChiTietSanPhamResponseDto>> UpdateAsync(int maChiTietSP, ChiTietSanPhamRequestDto dto)
        {
            if (dto == null)
                return BaseResponse<ChiTietSanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("Dto", "Dữ liệu không được null") },
                    "Cập nhật thất bại");

            var entity = await _repo.GetByIdAsync(maChiTietSP);
            if (entity == null)
                return BaseResponse<ChiTietSanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaChiTietSP", "Chi tiết sản phẩm không tồn tại") },
                    "Cập nhật thất bại");

            // Map DTO -> Entity
            entity.MaKichThuoc = dto.MaKichThuoc;
            entity.MaMauSac = dto.MaMauSac;
            entity.SoLuongTon = dto.SoLuongTon;
            entity.GiaBan = dto.GiaBan;

            var updated = await _repo.UpdateAsync(entity);
            var responseDto = await MapEntityToResponse(updated);

            return BaseResponse<ChiTietSanPhamResponseDto>.Success(responseDto, "Cập nhật chi tiết sản phẩm thành công");
        }

        /// Xóa chi tiết sản phẩm

        public async Task<BaseResponse<bool>> DeleteAsync(int maChiTietSP)
        {
            var entity = await _repo.GetByIdAsync(maChiTietSP);
            if (entity == null)
                return BaseResponse<bool>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaChiTietSP", "Chi tiết sản phẩm không tồn tại") },
                    "Xóa thất bại");

            var success = await _repo.DeleteAsync(maChiTietSP);
            return BaseResponse<bool>.Success(success, success ? "Xóa thành công" : "Xóa thất bại");
        }

        /// Lấy chi tiết sản phẩm theo ID

        public async Task<BaseResponse<ChiTietSanPhamResponseDto>> GetByIdAsync(int maChiTietSP)
        {
            var entity = await _repo.GetByIdAsync(maChiTietSP);
            if (entity == null)
                return BaseResponse<ChiTietSanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaChiTietSP", "Không tìm thấy") },
                    "Lấy dữ liệu thất bại");

            var responseDto = await MapEntityToResponse(entity);
            return BaseResponse<ChiTietSanPhamResponseDto>.Success(responseDto, "Lấy chi tiết sản phẩm thành công");
        }

        /// Lấy danh sách chi tiết sản phẩm theo ID sản phẩm

        public async Task<BaseResponse<List<ChiTietSanPhamResponseDto>>> GetBySanPhamIdAsync(int maSP)
        {
            var entities = await _repo.GetBySanPhamIdAsync(maSP);
            var dtos = new List<ChiTietSanPhamResponseDto>();

            foreach (var entity in entities)
            {
                dtos.Add(await MapEntityToResponse(entity));
            }

            return BaseResponse<List<ChiTietSanPhamResponseDto>>.Success(dtos, "Lấy danh sách chi tiết sản phẩm thành công");
        }

        /// Lấy tất cả chi tiết sản phẩm

        public async Task<BaseResponse<List<ChiTietSanPhamResponseDto>>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            var dtos = new List<ChiTietSanPhamResponseDto>();

            foreach (var entity in entities)
            {
                dtos.Add(await MapEntityToResponse(entity));
            }

            return BaseResponse<List<ChiTietSanPhamResponseDto>>.Success(dtos, "Lấy danh sách chi tiết sản phẩm thành công");
        }

        /// Map Entity -> Response DTO, bao gồm thông tin Kích Thước và Màu Sắc

        private async Task<ChiTietSanPhamResponseDto> MapEntityToResponse(ChiTietSanPham entity)
        {
            var kichThuoc = await _repoKichThuoc.GetByIdAsync(entity.MaKichThuoc);
            var mauSac = await _repoMauSac.GetByIdAsync(entity.MaMauSac);
            var sanPham = await _repoSanPham.GetByIdAsync(entity.MaSP);

            return new ChiTietSanPhamResponseDto
            {
                MaSP = entity.MaSP,
                MaChiTietSP = entity.MaChiTietSP,
                TenSanPham = sanPham?.TenSP ?? "N/A",
                MaKichThuoc = entity.MaKichThuoc,
                TenKichThuoc = kichThuoc?.TenKichThuoc ?? "N/A",
                MaMauSac = entity.MaMauSac,
                TenMauSac = mauSac?.TenMauSac ?? "N/A",
                SoLuongTon = entity.SoLuongTon,
                GiaBan = entity.GiaBan,
                NgayTao = entity.NgayTao
            };
        }
    }
}