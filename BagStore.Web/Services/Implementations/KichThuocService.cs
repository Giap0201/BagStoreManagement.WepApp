using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Models.Common;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;

namespace BagStore.Web.Services.Implementations
{
    public class KichThuocService : IKichThuocService
    {
        private readonly IKichThuocRepository _repo;

        public KichThuocService(IKichThuocRepository repo)
        {
            _repo = repo;
        }

        // Tạo mới kích thước
        public async Task<BaseResponse<KichThuocDto>> CreateAsync(KichThuocDto dto)
        {
            if (dto == null)
                return BaseResponse<KichThuocDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("Dto", "Dữ liệu không được null") },
                    "Tạo mới thất bại");

            // Kiểm tra duplicate tên
            var existing = await _repo.GetByNameAsync(dto.TenKichThuoc);
            if (existing != null)
                return BaseResponse<KichThuocDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail(nameof(dto.TenKichThuoc), $"Tên kích thước '{dto.TenKichThuoc}' đã tồn tại") },
                    "Tạo mới thất bại");

            var entity = new KichThuoc
            {
                TenKichThuoc = dto.TenKichThuoc,
                ChieuDai = dto.ChieuDai,
                ChieuRong = dto.ChieuRong,
                ChieuCao = dto.ChieuCao
            };

            var created = await _repo.AddAsync(entity);
            return BaseResponse<KichThuocDto>.Success(MapEntityToDto(created), "Tạo mới kích thước thành công");
        }

        // Cập nhật kích thước
        public async Task<BaseResponse<KichThuocDto>> UpdateAsync(int maKichThuoc, KichThuocDto dto)
        {
            var entity = await _repo.GetByIdAsync(maKichThuoc);
            if (entity == null)
                return BaseResponse<KichThuocDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaKichThuoc", "Không tìm thấy kích thước") },
                    "Cập nhật thất bại");

            // Kiểm tra duplicate tên khác record hiện tại
            var duplicate = await _repo.GetByNameAsync(dto.TenKichThuoc);
            if (duplicate != null && duplicate.MaKichThuoc != maKichThuoc)
                return BaseResponse<KichThuocDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail(nameof(dto.TenKichThuoc), $"Tên kích thước '{dto.TenKichThuoc}' đã tồn tại") },
                    "Cập nhật thất bại");

            entity.TenKichThuoc = dto.TenKichThuoc;
            entity.ChieuDai = dto.ChieuDai;
            entity.ChieuRong = dto.ChieuRong;
            entity.ChieuCao = dto.ChieuCao;

            var updated = await _repo.UpdateAsync(entity);
            return BaseResponse<KichThuocDto>.Success(MapEntityToDto(updated), "Cập nhật kích thước thành công");
        }

        // Xóa kích thước
        public async Task<BaseResponse<bool>> DeleteAsync(int maKichThuoc)
        {
            var entity = await _repo.GetByIdAsync(maKichThuoc);
            if (entity == null)
                return BaseResponse<bool>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaKichThuoc", "Không tìm thấy kích thước") },
                    "Xóa thất bại");

            var success = await _repo.DeleteAsync(maKichThuoc);
            return BaseResponse<bool>.Success(success, success ? "Xóa kích thước thành công" : "Xóa thất bại");
        }

        // Lấy kích thước theo ID
        public async Task<BaseResponse<KichThuocDto>> GetByIdAsync(int maKichThuoc)
        {
            var entity = await _repo.GetByIdAsync(maKichThuoc);
            if (entity == null)
                return BaseResponse<KichThuocDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaKichThuoc", "Không tìm thấy kích thước") },
                    "Lấy dữ liệu thất bại");

            return BaseResponse<KichThuocDto>.Success(MapEntityToDto(entity), "Lấy kích thước thành công");
        }

        // Lấy tất cả kích thước
        public async Task<BaseResponse<List<KichThuocDto>>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            var dtos = entities.Select(MapEntityToDto).ToList();
            return BaseResponse<List<KichThuocDto>>.Success(dtos, "Lấy danh sách kích thước thành công");
        }

        // Mapping entity -> DTO
        private KichThuocDto MapEntityToDto(KichThuoc entity)
        {
            return new KichThuocDto
            {
                MaKichThuoc = entity.MaKichThuoc,
                TenKichThuoc = entity.TenKichThuoc,
                ChieuDai = entity.ChieuDai,
                ChieuRong = entity.ChieuRong,
                ChieuCao = entity.ChieuCao
            };
        }
    }
}