using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;

namespace BagStore.Web.Services.Implementations
{
    public class DanhMucLoaiTuiService : IDanhMucLoaiTuiService
    {
        private readonly IDanhMucLoaiTuiRepository _repo;

        public DanhMucLoaiTuiService(IDanhMucLoaiTuiRepository repo)
        {
            _repo = repo;
        }

        public async Task<DanhMucLoaiTuiDto> CreateAsync(DanhMucLoaiTuiDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Dữ liệu không được null.");

            var entity = new DanhMucLoaiTui
            {
                TenLoaiTui = dto.TenLoaiTui,
                MoTa = dto.MoTa
            };

            var created = await _repo.AddAsync(entity);
            return MapEntityToDto(created);
        }

        public async Task<bool> DeleteAsync(int maLoaiTui)
        {
            var entity = await _repo.GetByIdAsync(maLoaiTui);
            if (entity == null)
                throw new KeyNotFoundException($"Loại túi với mã {maLoaiTui} không tồn tại.");

            return await _repo.DeleteAsync(maLoaiTui);
        }

        public async Task<List<DanhMucLoaiTuiDto>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(MapEntityToDto).ToList();
        }

        public async Task<DanhMucLoaiTuiDto> GetByIdAsync(int maLoaiTui)
        {
            var entity = await _repo.GetByIdAsync(maLoaiTui);
            if (entity == null)
                throw new KeyNotFoundException($"Loại túi với mã {maLoaiTui} không tồn tại.");

            return MapEntityToDto(entity);
        }

        public async Task<DanhMucLoaiTuiDto> UpdateAsync(int maLoaiTui, DanhMucLoaiTuiDto dto)
        {
            var entity = await _repo.GetByIdAsync(maLoaiTui);
            if (entity == null)
                throw new KeyNotFoundException($"Loại túi với mã {maLoaiTui} không tồn tại.");

            // Optional: kiểm tra dto.MaLoaiTui có khớp với maLoaiTui không
            if (dto.MaLoaiTui != maLoaiTui)
                throw new ArgumentException("Mã loại túi trong DTO không khớp với URL.");

            entity.TenLoaiTui = dto.TenLoaiTui;
            entity.MoTa = dto.MoTa;

            var updated = await _repo.UpdateAsync(entity);
            return MapEntityToDto(updated);
        }

        // Private helper method để mapping
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