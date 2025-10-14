using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;
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

        // Tạo mới thương hiệu
        public async Task<ThuongHieuDto> CreateAsync(ThuongHieuDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Dữ liệu không được null.");

            var entity = new ThuongHieu
            {
                TenThuongHieu = dto.TenThuongHieu,
                QuocGia = dto.QuocGia
            };

            var created = await _repo.AddAsync(entity);
            return MapEntityToDto(created);
        }

        // Lấy tất cả thương hiệu
        public async Task<List<ThuongHieuDto>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(MapEntityToDto).ToList();
        }

        // Lấy thương hiệu theo id
        public async Task<ThuongHieuDto> GetByIdAsync(int maThuongHieu)
        {
            var entity = await _repo.GetByIdAsync(maThuongHieu);
            if (entity == null)
                throw new KeyNotFoundException($"Thương hiệu với mã {maThuongHieu} không tồn tại.");

            return MapEntityToDto(entity);
        }

        // Cập nhật thương hiệu
        public async Task<ThuongHieuDto> UpdateAsync(int maThuongHieu, ThuongHieuDto dto)
        {
            var entity = await _repo.GetByIdAsync(maThuongHieu);
            if (entity == null)
                throw new KeyNotFoundException($"Thương hiệu với mã {maThuongHieu} không tồn tại.");

            if (dto.MaThuongHieu != maThuongHieu)
                throw new ArgumentException("Mã thương hiệu trong DTO không khớp với URL.");

            entity.TenThuongHieu = dto.TenThuongHieu;
            entity.QuocGia = dto.QuocGia;

            var updated = await _repo.UpdateAsync(entity);
            return MapEntityToDto(updated);
        }

        // Xóa thương hiệu
        public async Task<bool> DeleteAsync(int maThuongHieu)
        {
            var entity = await _repo.GetByIdAsync(maThuongHieu);
            if (entity == null)
                throw new KeyNotFoundException($"Thương hiệu với mã {maThuongHieu} không tồn tại.");

            return await _repo.DeleteAsync(maThuongHieu);
        }

        // Private helper mapping entity → DTO
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