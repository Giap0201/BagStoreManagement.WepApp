using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BagStore.Web.Services.Implementations
{
    public class ChiTietSanPhamService : IChiTietSanPhamService
    {
        private readonly IChiTietSanPhamRepository _repo;

        public ChiTietSanPhamService(IChiTietSanPhamRepository repo)
        {
            _repo = repo;
        }

        public async Task<ChiTietSanPhamResponseDto?> AddAsync(ChiTietSanPhamCreateDto dto)
        {
            //var sanPham = await _repo.GetByIdAsync(maChiTietSanPham)
            var entity = new ChiTietSanPham
            {
                MaMauSac = dto.MaMauSac,
                MaKichThuoc = dto.MaKichThuoc,
                GiaBan = dto.GiaBan,
                SoLuongTon = dto.SoLuongTon,
                NgayTao = DateTime.Now
            };
            var created = await _repo.AddAsync(entity);
            return new ChiTietSanPhamResponseDto
            {
                MaChiTietSP = created.MaChiTietSP,
                MaKichThuoc = created.MaKichThuoc,
                MaMauSac = created.MaMauSac,
                GiaBan = created.GiaBan,
                SoLuongTon = created.SoLuongTon
            };
        }

        public async Task<ChiTietSanPhamResponseDto?> GetByIdAsync(int maChiTietSanPham)
        {
            var result = await _repo.GetByIdAsync(maChiTietSanPham);
            if (result == null) return null;
            return new ChiTietSanPhamResponseDto
            {
                MaChiTietSP = result.MaChiTietSP,
                MaKichThuoc = result.MaKichThuoc,
                MaMauSac = result.MaMauSac,
                GiaBan = result.GiaBan,
                SoLuongTon = result.SoLuongTon
            };
        }
    }
}