using BagStore.Domain.Entities;
using BagStore.Web.Helpers;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;
using BagStore.Web.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;

namespace BagStore.Web.Services.Implementations
{
    public class SanPhamService : ISanPhamService
    {
        private readonly ISanPhamRepository _repo;
        private readonly IWebHostEnvironment _env;

        public SanPhamService(ISanPhamRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public async Task<SanPhamDetailResponseDto> CreateAsync(SanPhamCreateDto dto)
        {
            //validate du lieu
            if (dto.BienThe == null)
                throw new ArgumentException("Phải có ít nhất 1 biến thể sản phẩm.");

            if (dto.AnhChinh == null)
                throw new ArgumentException("Phải có ít nhất 1 ảnh sản phẩm.");

            //map dto to entity
            var sanPham = new SanPham
            {
                TenSP = dto.TenSP,
                MoTaChiTiet = dto.MoTaChiTiet,
                MetaTitle = dto.MetaTitle,
                MetaDescription = dto.MetaDescription,
                MaLoaiTui = dto.MaLoaiTui,
                MaChatLieu = dto.MaChatLieu,
                MaThuongHieu = dto.MaThuongHieu,
                NgayCapNhat = DateTime.Now,
                ChiTietSanPhams = new List<ChiTietSanPham>(),
                AnhSanPhams = new List<AnhSanPham>()
            };

            var ctsp = new ChiTietSanPham
            {
                MaKichThuoc = dto.BienThe.MaKichThuoc,
                MaMauSac = dto.BienThe.MaMauSac,
                SoLuongTon = dto.BienThe.SoLuongTon,
                GiaBan = dto.BienThe.GiaBan,
                NgayTao = DateTime.Now
            };
            sanPham.ChiTietSanPhams.Add(ctsp);

            //upload anh
            var anhChinh = await ImageHelper.UploadSingleImageAsync(
                dto.AnhChinh,
                _env.WebRootPath
                );

            if (anhChinh != null)
            {
                sanPham.AnhSanPhams.Add(anhChinh);
            }

            //luu vao database
            var created = await _repo.AddAsync(sanPham);

            //map sao dto response
            var response = new SanPhamDetailResponseDto
            {
                MaSP = sanPham.MaSP,
                TenSP = sanPham.TenSP,
                MoTaChiTiet = sanPham.MoTaChiTiet,
                MetaTitle = sanPham.MetaTitle,
                MetaDescription = sanPham.MetaDescription,
                MaLoaiTui = sanPham.MaLoaiTui,
                //TenLoaiTui = sanPham.DanhMucLoaiTui != null ? sanPham.DanhMucLoaiTui.TenLoaiTui : "N/A",
                MaThuongHieu = sanPham.MaThuongHieu,
                //TenThuongHieu = sanPham.ThuongHieu != null ? sanPham.ThuongHieu.TenThuongHieu : "N/A",
                MaChatLieu = sanPham.MaChatLieu,
                //TenChatLieu = sanPham.ChatLieu != null ? sanPham.ChatLieu.TenChatLieu : "N/A",
                NgayCapNhat = sanPham.NgayCapNhat,
                ChiTietSanPhams = sanPham.ChiTietSanPhams.Select(ct => new ChiTietSanPhamResponseDto
                {
                    MaChiTietSP = ct.MaChiTietSP,
                    MaKichThuoc = ct.MaKichThuoc,
                    //TenKichThuoc = ct.KichThuoc != null ? ct.KichThuoc.TenKichThuoc : "N/A",
                    MaMauSac = ct.MaMauSac,
                    //TenMauSac = ct.MauSac != null ? ct.MauSac.TenMauSac : "N/A",
                    SoLuongTon = ct.SoLuongTon,
                    GiaBan = ct.GiaBan
                }).ToList(),
                AnhSanPhams = sanPham.AnhSanPhams.Select(a => new AnhSanPhamResponseDto
                {
                    MaAnh = a.MaAnh,
                    DuongDan = a.DuongDan,
                    ThuTuHienThi = a.ThuTuHienThi,
                    LaHinhChinh = a.LaHinhChinh
                }).ToList()
            };

            return response;
        }

        public async Task<SanPhamDetailResponseDto> GetByIdAsync(int maSP)
        {
            var sp = await _repo.GetByIdAsync(maSP);
            if (sp == null)
            {
                return null;
            }

            // map sang dto
            var dto = MappingResponse(sp);
            return dto;
        }

        public static SanPhamDetailResponseDto MappingResponse(SanPham sanPham)
        {
            return new SanPhamDetailResponseDto
            {
                MaSP = sanPham.MaSP,
                TenSP = sanPham.TenSP,
                MoTaChiTiet = sanPham.MoTaChiTiet,
                MetaTitle = sanPham.MetaTitle,
                MetaDescription = sanPham.MetaDescription,
                //MaLoaiTui = sanPham.MaLoaiTui,
                TenLoaiTui = sanPham.DanhMucLoaiTui != null ? sanPham.DanhMucLoaiTui.TenLoaiTui : "N/A",
                //MaThuongHieu = sanPham.MaThuongHieu,
                TenThuongHieu = sanPham.ThuongHieu != null ? sanPham.ThuongHieu.TenThuongHieu : "N/A",
                //MaChatLieu = sanPham.MaChatLieu,
                TenChatLieu = sanPham.ChatLieu != null ? sanPham.ChatLieu.TenChatLieu : "N/A",
                NgayCapNhat = sanPham.NgayCapNhat,
                ChiTietSanPhams = sanPham.ChiTietSanPhams.Select(ct => new ChiTietSanPhamResponseDto
                {
                    MaChiTietSP = ct.MaChiTietSP,
                    //MaKichThuoc = ct.MaKichThuoc,
                    TenKichThuoc = ct.KichThuoc != null ? ct.KichThuoc.TenKichThuoc : "N/A",
                    //MaMauSac = ct.MaMauSac,
                    TenMauSac = ct.MauSac != null ? ct.MauSac.TenMauSac : "N/A",
                    SoLuongTon = ct.SoLuongTon,
                    GiaBan = ct.GiaBan
                }).ToList(),
                AnhSanPhams = sanPham.AnhSanPhams.Select(a => new AnhSanPhamResponseDto
                {
                    //MaAnh = a.MaAnh,
                    DuongDan = a.DuongDan,
                    ThuTuHienThi = a.ThuTuHienThi,
                    LaHinhChinh = a.LaHinhChinh
                }).ToList()
            };
        }

        public async Task<SanPhamDetailResponseDto> UpdateAsync(int maSP, SanPhamUpdateDto dto)
        {
            var sp = await _repo.GetByIdAsync(maSP);
            if (sp != null) throw new KeyNotFoundException("Sản phẩm không tồn tại");
            sp.TenSP = dto.TenSP;
            sp.MoTaChiTiet = dto.MoTaChiTiet;
            sp.MetaDescription = dto.MetaDescription;
            sp.MetaTitle = dto.MetaTitle;
            sp.MaLoaiTui = dto.MaLoaiTui;
            sp.MaThuongHieu = dto.MaThuongHieu;
            sp.MaChatLieu = dto.MaChatLieu;

            var updated = await _repo.UpdateAsync(sp);
            var response = new SanPhamDetailResponseDto
            {
                MaSP = updated.MaSP,
                TenSP = updated.TenSP,
                MoTaChiTiet = updated.MoTaChiTiet,
                MetaTitle = updated.MetaTitle,
                MetaDescription = updated.MetaDescription,
                MaLoaiTui = updated.MaLoaiTui,
                //TenLoaiTui = sanPham.DanhMucLoaiTui != null ? sanPham.DanhMucLoaiTui.TenLoaiTui : "N/A",
                MaThuongHieu = updated.MaThuongHieu,
                //TenThuongHieu = sanPham.ThuongHieu != null ? sanPham.ThuongHieu.TenThuongHieu : "N/A",
                MaChatLieu = updated.MaChatLieu,
                //TenChatLieu = sanPham.ChatLieu != null ? sanPham.ChatLieu.TenChatLieu : "N/A",
                NgayCapNhat = updated.NgayCapNhat,
                ChiTietSanPhams = updated.ChiTietSanPhams.Select(ct => new ChiTietSanPhamResponseDto
                {
                    MaChiTietSP = ct.MaChiTietSP,
                    MaKichThuoc = ct.MaKichThuoc,
                    //TenKichThuoc = ct.KichThuoc != null ? ct.KichThuoc.TenKichThuoc : "N/A",
                    MaMauSac = ct.MaMauSac,
                    //TenMauSac = ct.MauSac != null ? ct.MauSac.TenMauSac : "N/A",
                    SoLuongTon = ct.SoLuongTon,
                    GiaBan = ct.GiaBan
                }).ToList(),
                AnhSanPhams = updated.AnhSanPhams.Select(a => new AnhSanPhamResponseDto
                {
                    MaAnh = a.MaAnh,
                    DuongDan = a.DuongDan,
                    ThuTuHienThi = a.ThuTuHienThi,
                    LaHinhChinh = a.LaHinhChinh
                }).ToList()
            };

            return response;
        }

        public async Task<bool> DeleteAsync(int maSP)
        {
            var sp = await _repo.GetByIdAsync(maSP);
            if (sp == null) throw new KeyNotFoundException("Sản phẩm không tồn tại");
            return await _repo.DeleteAsync(maSP);
        }
    }
}