using BagStore.Domain.Entities.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class SanPham
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; } = null!;
        public decimal GiaBan { get; set; }
        public decimal? GiaGoc { get; set; }
        public string? MoTaChiTiet { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

        public int MaLoaiTui { get; set; }
        public int MaThuongHieu { get; set; }
        public int MaChatLieu { get; set; }

        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        // FK Nhân viên quản lý sản phẩm
        public string NhanVienCapNhatId { get; set; } = null!;

        // Navigation properties
        public DanhMucLoaiTui DanhMucLoaiTui { get; set; } = null!;

        public ThuongHieu ThuongHieu { get; set; } = null!;
        public ChatLieu ChatLieu { get; set; } = null!;
        public NhanVienProfile NhanVienCapNhat { get; set; } = null!;

        public ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();
        public ICollection<AnhSanPham> AnhSanPhams { get; set; } = new List<AnhSanPham>();
        public ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();
    }
}