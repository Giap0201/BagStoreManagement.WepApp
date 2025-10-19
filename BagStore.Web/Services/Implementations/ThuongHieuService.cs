using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Models.Common;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;

namespace BagStore.Web.Services.Implementations
{
    public class ThuongHieuService : IThuongHieuService
    {
        private readonly IThuongHieuRepository _repo;

        public ThuongHieuService(IThuongHieuRepository repo)
        {
            _repo = repo;
        }

        // Thêm mới thương hiệu
        public async Task<BaseResponse<ThuongHieuDto>> CreateAsync(ThuongHieuDto dto)
        {
            // Kiểm tra duplicate tên
            var existing = await _repo.GetByNameAsync(dto.TenThuongHieu);
            if (existing != null)
                return BaseResponse<ThuongHieuDto>.Error(
                    new List<ErrorDetail>
                    {
                        new ErrorDetail(nameof(dto.TenThuongHieu), $"Tên thương hiệu '{dto.TenThuongHieu}' đã tồn tại")
                    },
                    "Tạo mới thất bại");

            var entity = new ThuongHieu
            {
                TenThuongHieu = dto.TenThuongHieu,
                QuocGia = dto.QuocGia
            };

            var created = await _repo.AddAsync(entity);
            return BaseResponse<ThuongHieuDto>.Success(MapEntityToDto(created), "Tạo mới thương hiệu thành công");
        }

        // Cập nhật thương hiệu
        public async Task<BaseResponse<ThuongHieuDto>> UpdateAsync(int maThuongHieu, ThuongHieuDto dto)
        {
            var entity = await _repo.GetByIdAsync(maThuongHieu);
            if (entity == null)
                return BaseResponse<ThuongHieuDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaThuongHieu", "Không tìm thấy thương hiệu") },
                    "Cập nhật thất bại");

            // Kiểm tra duplicate tên khác record hiện tại
            var duplicate = await _repo.GetByNameAsync(dto.TenThuongHieu);
            if (duplicate != null && duplicate.MaThuongHieu != maThuongHieu)
                return BaseResponse<ThuongHieuDto>.Error(
                    new List<ErrorDetail>
                    {
                        new ErrorDetail(nameof(dto.TenThuongHieu), $"Tên thương hiệu '{dto.TenThuongHieu}' đã tồn tại")
                    },
                    "Cập nhật thất bại");

            entity.TenThuongHieu = dto.TenThuongHieu;
            entity.QuocGia = dto.QuocGia;

            var updated = await _repo.UpdateAsync(entity);
            return BaseResponse<ThuongHieuDto>.Success(MapEntityToDto(updated), "Cập nhật thương hiệu thành công");
        }

        // Xóa thương hiệu
        public async Task<BaseResponse<bool>> DeleteAsync(int maThuongHieu)
        {
            var entity = await _repo.GetByIdAsync(maThuongHieu);
            if (entity == null)
                return BaseResponse<bool>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaThuongHieu", "Không tìm thấy thương hiệu") },
                    "Xóa thất bại");

            var success = await _repo.DeleteAsync(maThuongHieu);
            return BaseResponse<bool>.Success(success, success ? "Xóa thương hiệu thành công" : "Xóa thất bại");
        }

        // Lấy thương hiệu theo ID
        public async Task<BaseResponse<ThuongHieuDto>> GetByIdAsync(int maThuongHieu)
        {
            var entity = await _repo.GetByIdAsync(maThuongHieu);
            if (entity == null)
                return BaseResponse<ThuongHieuDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaThuongHieu", "Không tìm thấy thương hiệu") },
                    "Lấy dữ liệu thất bại");

            return BaseResponse<ThuongHieuDto>.Success(MapEntityToDto(entity), "Lấy thương hiệu thành công");
        }

        // Lấy tất cả thương hiệu
        public async Task<BaseResponse<List<ThuongHieuDto>>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            var dtos = entities.Select(MapEntityToDto).ToList();
            return BaseResponse<List<ThuongHieuDto>>.Success(dtos, "Lấy danh sách thương hiệu thành công");
        }

        // Mapping entity -> DTO
        private ThuongHieuDto MapEntityToDto(ThuongHieu entity)
        {
            return new ThuongHieuDto
            {
                MaThuongHieu = entity.MaThuongHieu,
                TenThuongHieu = entity.TenThuongHieu,
                QuocGia = entity.QuocGia
            };
        }
    }
}