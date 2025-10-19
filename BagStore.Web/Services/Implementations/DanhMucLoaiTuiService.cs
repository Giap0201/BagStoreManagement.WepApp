using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Services.Interfaces;
using BagStore.Web.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace BagStore.Services.Implementations
{
    public class DanhMucLoaiTuiService : IDanhMucLoaiTuiService
    {
        private readonly IDanhMucLoaiTuiRepository _repo;

        public DanhMucLoaiTuiService(IDanhMucLoaiTuiRepository repo)
        {
            _repo = repo;
        }

        private List<ErrorDetail> ValidateDto(DanhMucLoaiTuiDto dto)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(dto, null, null);
            Validator.TryValidateObject(dto, context, validationResults, true);
            return validationResults.Select(vr =>
                new ErrorDetail(vr.MemberNames.FirstOrDefault() ?? "", vr.ErrorMessage)).ToList();
        }

        // Thêm mới loại túi
        public async Task<BaseResponse<DanhMucLoaiTuiDto>> CreateAsync(DanhMucLoaiTuiDto dto)
        {
            var errors = ValidateDto(dto);
            if (errors.Any())
            {
                return BaseResponse<DanhMucLoaiTuiDto>.Error(errors);
            }
            var entity = new DanhMucLoaiTui
            {
                TenLoaiTui = dto.TenLoaiTui!,
                MoTa = dto.MoTa!
            };
            var created = await _repo.AddAsync(entity);
            var resultDto = MapEntityToDto(created);
            return BaseResponse<DanhMucLoaiTuiDto>.Success(resultDto);
        }

        // Cập nhật loại túi
        public async Task<BaseResponse<DanhMucLoaiTuiDto>> UpdateAsync(int maLoaiTui, DanhMucLoaiTuiDto dto)
        {
            var errors = ValidateDto(dto);
            if (errors.Any())
            {
                return BaseResponse<DanhMucLoaiTuiDto>.Error(errors);
            }
            var existing = await _repo.GetByIdAsync(maLoaiTui);
            if (existing == null)
            {
                return BaseResponse<DanhMucLoaiTuiDto>.Error(new List<ErrorDetail>
                {
                    new ErrorDetail("MaLoaiTui", $"Loại túi với mã {maLoaiTui} không tồn tại.")
                });
            }
            existing.TenLoaiTui = dto.TenLoaiTui;
            existing.MoTa = dto.MoTa;
            var updated = await _repo.UpdateAsync(existing);
            var resultDto = MapEntityToDto(updated);
            return BaseResponse<DanhMucLoaiTuiDto>.Success(resultDto, "Cập nhật thành công");
        }

        // Xóa loại túi
        public async Task<BaseResponse<bool>> DeleteAsync(int maLoaiTui)
        {
            var existing = await _repo.GetByIdAsync(maLoaiTui);
            if (existing == null)
            {
                return BaseResponse<bool>.Error(new List<ErrorDetail>
                {
                    new ErrorDetail("MaLoaiTui", $"Loại túi với mã {maLoaiTui} không tồn tại.")
                });
            }
            var success = await _repo.DeleteAsync(maLoaiTui);
            return BaseResponse<bool>.Success(success, success ? "Xóa loại túi thành công." : "Xoá thất bại");
        }

        // Lấy loại túi theo ID
        public async Task<BaseResponse<DanhMucLoaiTuiDto>> GetByIdAsync(int maLoaiTui)
        {
            var entity = await _repo.GetByIdAsync(maLoaiTui);
            if (entity == null)
            {
                return BaseResponse<DanhMucLoaiTuiDto>.Error(new List<ErrorDetail>
                {
                    new ErrorDetail("MaLoaiTui", $"Loại túi với mã {maLoaiTui} không tồn tại.")
                });
            }
            var dto = MapEntityToDto(entity);
            return BaseResponse<DanhMucLoaiTuiDto>.Success(dto);
        }

        // Lấy tất cả loại túi
        public async Task<BaseResponse<List<DanhMucLoaiTuiDto>>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            var dtos = entities.Select(MapEntityToDto).ToList();
            return BaseResponse<List<DanhMucLoaiTuiDto>>.Success(dtos, "Lấy danh sách loại túi thành công.");
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