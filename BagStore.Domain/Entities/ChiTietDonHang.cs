using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class ChiTietDonHang
    {
        public int MaChiTietDonHang { get; set; }
        public int MaDonHang { get; set; }
        public int MaChiTietSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public DonHang DonHang { get; set; }
        public ChiTietSanPham ChiTietSanPham { get; set; }
    }
}