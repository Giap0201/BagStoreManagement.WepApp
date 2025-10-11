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
        public int SoLuongTon { get; set; }
        public decimal GiaBan { get; set; }
        public DateTime NgayTao { get; set; }

        // Quan hệ
        public SanPham SanPham { get; set; }

        public KichThuoc KichThuoc { get; set; }
        public MauSac MauSac { get; set; }

        public ICollection<GioHang> GioHangs { get; set; }
        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
    }
}