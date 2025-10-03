using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class ChiTietSanPham
    {
        public int MaChiTietSP { get; set; }
        public int MaSP { get; set; }
        public int MaKichThuoc { get; set; }
        public int MaMauSac { get; set; }

        // Tồn kho tổng được tính toán từ bảng TonKho.

        public decimal GiaBan { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties
        public SanPham SanPham { get; set; } = null!;

        public KichThuoc KichThuoc { get; set; } = null!;
        public MauSac MauSac { get; set; } = null!;

        public ICollection<AnhSanPham> AnhSanPhams { get; set; } = new List<AnhSanPham>();
        public ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
        public ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();
        public ICollection<TonKho> TonKhos { get; set; } = new List<TonKho>();
    }
}