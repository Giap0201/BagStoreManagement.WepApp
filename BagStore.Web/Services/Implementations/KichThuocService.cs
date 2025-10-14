using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;
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

        public async Task<KichThuocDto> CreateAsync(KichThuocDto dto)
        {
            if (dto == null) throw new ArgumentException("Dữ liệu không hợp lệ");

            var entity = new KichThuoc
            {
                TenKichThuoc = dto.TenKichThuoc,
                ChieuDai = dto.ChieuDai,
                ChieuRong = dto.ChieuRong,
                ChieuCao = dto.ChieuCao
            };

            var result = await _repo.AddAsync(entity);
            return new KichThuocDto
            {
                MaKichThuoc = result.MaKichThuoc,
                TenKichThuoc = result.TenKichThuoc,
                ChieuDai = result.ChieuDai,
                ChieuRong = result.ChieuRong,
                ChieuCao = result.ChieuCao
            };
        }

        public async Task<bool> DeleteAsync(int maKichThuoc)
        {
            var entity = await _repo.GetByIdAsync(maKichThuoc);
            if (entity == null) throw new ArgumentException($"Kích thước với mã {maKichThuoc} không tồn tại");
            return await _repo.DeleteAsync(maKichThuoc);
        }

        public async Task<List<KichThuocDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(e => new KichThuocDto
            {
                MaKichThuoc = e.MaKichThuoc,
                TenKichThuoc = e.TenKichThuoc,
                ChieuDai = e.ChieuDai,
                ChieuRong = e.ChieuRong,
                ChieuCao = e.ChieuCao
            }).ToList();
        }

        public async Task<KichThuocDto> GetByIdAsync(int maKichThuoc)
        {
            var entity = await _repo.GetByIdAsync(maKichThuoc);
            if (entity == null) throw new ArgumentException($"Kích thước với mã {maKichThuoc} không tồn tại");
            return new KichThuocDto
            {
                MaKichThuoc = entity.MaKichThuoc,
                TenKichThuoc = entity.TenKichThuoc,
                ChieuDai = entity.ChieuDai,
                ChieuRong = entity.ChieuRong,
                ChieuCao = entity.ChieuCao
            };
        }

        public async Task<KichThuocDto> UpdateAsync(int maKichThuoc, KichThuocDto dto)
        {
            var entity = await _repo.GetByIdAsync(maKichThuoc);
            if (entity == null) throw new ArgumentException($"Kích thước với mã {maKichThuoc} không tồn tại");

            entity.TenKichThuoc = dto.TenKichThuoc;
            entity.ChieuDai = dto.ChieuDai;
            entity.ChieuRong = dto.ChieuRong;
            entity.ChieuCao = dto.ChieuCao;

            var result = await _repo.UpdateAsync(entity);
            return new KichThuocDto
            {
                MaKichThuoc = result.MaKichThuoc,
                TenKichThuoc = result.TenKichThuoc,
                ChieuDai = result.ChieuDai,
                ChieuRong = result.ChieuRong,
                ChieuCao = result.ChieuCao
            };
        }
    }
}