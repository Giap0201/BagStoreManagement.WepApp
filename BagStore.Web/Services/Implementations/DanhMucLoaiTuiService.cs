using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Services.Interfaces;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;

namespace BagStore.Services.Implementations
{
    public class DanhMucLoaiTuiService : IDanhMucLoaiTuiService
    {
        private readonly IDanhMucLoaiTuiRepository _repo;

        public DanhMucLoaiTuiService(IDanhMucLoaiTuiRepository repo)
        {
            _repo = repo;
        }

        // Tạo mới loại túi
        public async Task<BaseResponse<DanhMucLoaiTuiDto>> CreateAsync(DanhMucLoaiTuiDto dto)
        {
            // Kiểm tra duplicate tên
            var existing = await _repo.GetByNameAsync(dto.TenLoaiTui);
            if (existing != null)
                return BaseResponse<DanhMucLoaiTuiDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail(nameof(dto.TenLoaiTui), "Tên loại túi đã tồn tại") },
                    "Tạo mới thất bại");

            var entity = new DanhMucLoaiTui
            {
                TenLoaiTui = dto.TenLoaiTui,
                MoTa = dto.MoTa
            };

            var created = await _repo.AddAsync(entity);
            return BaseResponse<DanhMucLoaiTuiDto>.Success(MapEntityToDto(created), "Tạo mới thành công");
        }

        // Cập nhật loại túi
        public async Task<BaseResponse<DanhMucLoaiTuiDto>> UpdateAsync(int maLoaiTui, DanhMucLoaiTuiDto dto)
        {
            var entity = await _repo.GetByIdAsync(maLoaiTui);
            if (entity == null)
                return BaseResponse<DanhMucLoaiTuiDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaLoaiTui", "Không tìm thấy loại túi") },
                    "Cập nhật thất bại");

            // Kiểm tra duplicate tên khác record hiện tại
            var duplicate = await _repo.GetByNameAsync(dto.TenLoaiTui);
            if (duplicate != null && duplicate.MaLoaiTui != maLoaiTui)
                return BaseResponse<DanhMucLoaiTuiDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail(nameof(dto.TenLoaiTui), "Tên loại túi đã tồn tại") },
                    "Cập nhật thất bại");

            entity.TenLoaiTui = dto.TenLoaiTui;
            entity.MoTa = dto.MoTa;

            var updated = await _repo.UpdateAsync(entity);
            return BaseResponse<DanhMucLoaiTuiDto>.Success(MapEntityToDto(updated), "Cập nhật thành công");
        }

        // Xóa loại túi
        public async Task<BaseResponse<bool>> DeleteAsync(int maLoaiTui)
        {
            var entity = await _repo.GetByIdAsync(maLoaiTui);
            if (entity == null)
                return BaseResponse<bool>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaLoaiTui", "Không tìm thấy loại túi") },
                    "Xóa thất bại");

            var success = await _repo.DeleteAsync(maLoaiTui);
            return BaseResponse<bool>.Success(success, success ? "Xóa thành công" : "Xóa thất bại");
        }

        // Lấy loại túi theo ID
        public async Task<BaseResponse<DanhMucLoaiTuiDto>> GetByIdAsync(int maLoaiTui)
        {
            var entity = await _repo.GetByIdAsync(maLoaiTui);
            if (entity == null)
                return BaseResponse<DanhMucLoaiTuiDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaLoaiTui", "Không tìm thấy loại túi") },
                    "Lấy dữ liệu thất bại");

            return BaseResponse<DanhMucLoaiTuiDto>.Success(MapEntityToDto(entity), "Lấy thành công");
        }

        // Lấy tất cả loại túi
        public async Task<BaseResponse<List<DanhMucLoaiTuiDto>>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            var dtos = entities.Select(MapEntityToDto).ToList();
            return BaseResponse<List<DanhMucLoaiTuiDto>>.Success(dtos, "Lấy danh sách thành công");
        }

        // Mapping entity -> DTO
        private DanhMucLoaiTuiDto MapEntityToDto(DanhMucLoaiTui entity)
        {
            return new DanhMucLoaiTuiDto
            {
                MaLoaiTui = entity.MaLoaiTui,
                TenLoaiTui = entity.TenLoaiTui,
                MoTa = entity.MoTa
            };
        }
    }
}