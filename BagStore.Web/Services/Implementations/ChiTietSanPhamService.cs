using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;

namespace BagStore.Web.Services.Implementations
{
    public class ChiTietSanPhamService : IChiTietSanPhamService
    {
        private readonly IChiTietSanPhamRepository _repo;
        private readonly ISanPhamRepository _sanPhamRepo;

        public ChiTietSanPhamService(IChiTietSanPhamRepository repo, ISanPhamRepository sanPhamRepo)
        {
            _repo = repo;
            _sanPhamRepo = sanPhamRepo;
        }

        //them mot bien the moi vao san pham co san
        public async Task<ChiTietSanPhamResponseDto?> AddAsync(int maSP, ChiTietSanPhamCreateDto dto)
        {
            if (dto == null) throw new ArgumentException("Dữ liệu không hợp lệ");
            //kiem tra xem san pham cha ton tai chua

            var sanPham = await _sanPhamRepo.GetByIdAsync(maSP);
            if (sanPham == null) throw new KeyNotFoundException("Sản phẩm không tồn tại");

            //kiem tra xem mau sac va kich thuoc da ton tai chua
            var existing = await _repo.FindByAttributesAsync(maSP, dto.MaMauSac, dto.MaKichThuoc);
            if (existing != null) throw new InvalidOperationException("Biến thể với màu sắc và kích thước này đã tồn tại");
            //tao moi bien the
            var entity = new ChiTietSanPham
            {
                MaSP = maSP,
                MaMauSac = dto.MaMauSac,
                MaKichThuoc = dto.MaKichThuoc,
                GiaBan = dto.GiaBan,
                SoLuongTon = dto.SoLuongTon,
                NgayTao = DateTime.Now
            };
            var created = await _repo.AddAsync(entity);
            return new ChiTietSanPhamResponseDto
            {
                MaSP = created.MaSP,
                MaChiTietSP = created.MaChiTietSP,
                MaKichThuoc = created.MaKichThuoc,
                MaMauSac = created.MaMauSac,
                GiaBan = created.GiaBan,
                NgayTao = created.NgayTao,
                SoLuongTon = created.SoLuongTon
            };
        }

        public async Task<bool> DeleteAsync(int maChiTietSanPham)
        {
            var ct = await _repo.GetByIdAsync(maChiTietSanPham);
            if (ct == null) throw new KeyNotFoundException("Biến thể không tồn tại");
            return await _repo.DeleteAsync(maChiTietSanPham);
        }

        public async Task<ChiTietSanPhamResponseDto?> GetByIdAsync(int maChiTietSanPham)
        {
            var result = await _repo.GetByIdAsync(maChiTietSanPham);
            if (result == null) return null;
            return new ChiTietSanPhamResponseDto
            {
                MaSP = result.MaSP,
                MaChiTietSP = result.MaChiTietSP,
                TenSanPham = result.SanPham.TenSP,
                MaKichThuoc = result.MaKichThuoc,
                TenKichThuoc = result.KichThuoc.TenKichThuoc,
                MaMauSac = result.MaMauSac,
                TenMauSac = result.MauSac.TenMauSac,
                GiaBan = result.GiaBan,
                NgayTao = result.NgayTao,
                SoLuongTon = result.SoLuongTon
            };
        }

        public Task<List<ChiTietSanPham>> GetBySanPhamIdAsync(int maSP)
        {
            throw new NotImplementedException();
        }

        public async Task<ChiTietSanPhamResponseDto> UpdateAsync(int maChiTietSanPham, ChiTietSanPhamCreateDto dto)
        {
            var ct = await _repo.GetByIdAsync(maChiTietSanPham);
            if (ct == null) throw new KeyNotFoundException("Biến thể không tồn tại");
            ct.GiaBan = dto.GiaBan;
            ct.MaKichThuoc = dto.MaKichThuoc;
            ct.MaMauSac = dto.MaMauSac;
            ct.NgayTao = DateTime.Now;
            ct.SoLuongTon = dto.SoLuongTon;
            var updated = await _repo.UpdateAsync(ct);
            return new ChiTietSanPhamResponseDto
            {
                MaChiTietSP = updated.MaChiTietSP,
                MaKichThuoc = updated.MaKichThuoc,
                MaMauSac = updated.MaMauSac,
                GiaBan = updated.GiaBan,
                NgayTao = updated.NgayTao,
                SoLuongTon = updated.SoLuongTon
            };
        }
    }
}