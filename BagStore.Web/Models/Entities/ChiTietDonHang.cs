using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class ChiTietDonHang
    {
        public int MaChiTietDH { get; set; }
        public int MaDonHang { get; set; }
        public int MaChiTietSP { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }

        // Quan hệ
        public DonHang DonHang { get; set; }

        public ChiTietSanPham ChiTietSanPham { get; set; }
    }
}