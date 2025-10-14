using BagStore.Domain.Entities;
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

        public async Task<MauSacDto> CreateAsync(MauSacDto dto)
        {
            if (dto == null) throw new ArgumentException("Dữ liệu không hợp lệ");
            var entity = new MauSac { TenMauSac = dto.TenMauSac };
            var result = await _repo.AddAsync(entity);
            return new MauSacDto
            {
                MaMauSac = result.MaMauSac,
                TenMauSac = result.TenMauSac
            };
        }

        public async Task<bool> DeleteAsync(int maMauSac)
        {
            var entity = await _repo.GetByIdAsync(maMauSac);
            if (entity == null) throw new ArgumentException($"Màu sắc với mã {maMauSac} không tồn tại");
            return await _repo.DeleteAsync(maMauSac);
        }

        public async Task<List<MauSacDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(e => new MauSacDto
            {
                MaMauSac = e.MaMauSac,
                TenMauSac = e.TenMauSac
            }).ToList();
        }

        public async Task<MauSacDto> GetByIdAsync(int maMauSac)
        {
            var entity = await _repo.GetByIdAsync(maMauSac);
            if (entity == null) throw new ArgumentException($"Màu sắc với mã {maMauSac} không tồn tại");
            return new MauSacDto
            {
                MaMauSac = entity.MaMauSac,
                TenMauSac = entity.TenMauSac
            };
        }

        public async Task<MauSacDto> UpdateAsync(int maMauSac, MauSacDto dto)
        {
            var entity = await _repo.GetByIdAsync(maMauSac);
            if (entity == null) throw new ArgumentException($"Màu sắc với mã {maMauSac} không tồn tại");
            entity.TenMauSac = dto.TenMauSac;
            var result = await _repo.UpdateAsync(entity);
            return new MauSacDto
            {
                MaMauSac = result.MaMauSac,
                TenMauSac = result.TenMauSac
            };
        }
    }
}