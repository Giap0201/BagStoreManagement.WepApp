using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;

namespace BagStore.Web.Services.Implementations
{
    public class MauSacService : IMauSacService
    {
        private readonly IMauSacRepository _repo;

        public MauSacService(IMauSacRepository repo)
        {
            _repo = repo;
        }

        // Tạo mới màu sắc
        public async Task<BaseResponse<MauSacDto>> CreateAsync(MauSacDto dto)
        {
            // Kiểm tra duplicate
            var existing = await _repo.GetByNameAsync(dto.TenMauSac);
            if (existing != null)
            {
                return BaseResponse<MauSacDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail(nameof(dto.TenMauSac), $"Tên màu '{dto.TenMauSac}' đã tồn tại") },
                    "Tạo mới thất bại");
            }

            var entity = new MauSac
            {
                TenMauSac = dto.TenMauSac
            };

            var created = await _repo.AddAsync(entity);
            return BaseResponse<MauSacDto>.Success(MapEntityToDto(created), "Tạo mới thành công");
        }

        // Cập nhật màu sắc
        public async Task<BaseResponse<MauSacDto>> UpdateAsync(int maMauSac, MauSacDto dto)
        {
            var entity = await _repo.GetByIdAsync(maMauSac);
            if (entity == null)
            {
                return BaseResponse<MauSacDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaMauSac", $"Màu sắc với mã '{maMauSac}' không tồn tại") },
                    "Cập nhật thất bại");
            }

            // Kiểm tra duplicate với record khác
            var duplicate = await _repo.GetByNameAsync(dto.TenMauSac);
            if (duplicate != null && duplicate.MaMauSac != maMauSac)
            {
                return BaseResponse<MauSacDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail(nameof(dto.TenMauSac), $"Tên màu '{dto.TenMauSac}' đã tồn tại") },
                    "Cập nhật thất bại");
            }

            entity.TenMauSac = dto.TenMauSac;

            var updated = await _repo.UpdateAsync(entity);
            return BaseResponse<MauSacDto>.Success(MapEntityToDto(updated), "Cập nhật thành công");
        }

        // Xóa màu sắc
        public async Task<BaseResponse<bool>> DeleteAsync(int maMauSac)
        {
            var entity = await _repo.GetByIdAsync(maMauSac);
            if (entity == null)
            {
                return BaseResponse<bool>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaMauSac", $"Màu sắc với mã '{maMauSac}' không tồn tại") },
                    "Xóa thất bại");
            }

            var success = await _repo.DeleteAsync(maMauSac);
            return BaseResponse<bool>.Success(success, success ? "Xóa thành công" : "Xóa thất bại");
        }

        // Lấy màu sắc theo ID
        public async Task<BaseResponse<MauSacDto>> GetByIdAsync(int maMauSac)
        {
            var entity = await _repo.GetByIdAsync(maMauSac);
            if (entity == null)
            {
                return BaseResponse<MauSacDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaMauSac", $"Màu sắc với mã '{maMauSac}' không tồn tại") },
                    "Lấy dữ liệu thất bại");
            }

            return BaseResponse<MauSacDto>.Success(MapEntityToDto(entity), "Lấy dữ liệu thành công");
        }

        // Lấy tất cả màu sắc
        public async Task<BaseResponse<List<MauSacDto>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            var dtos = list.Select(MapEntityToDto).ToList();
            return BaseResponse<List<MauSacDto>>.Success(dtos, "Lấy danh sách màu sắc thành công");
        }

        // Mapping entity -> DTO
        private MauSacDto MapEntityToDto(MauSac entity)
        {
            return new MauSacDto
            {
                MaMauSac = entity.MaMauSac,
                TenMauSac = entity.TenMauSac
            };
        }
    }
}