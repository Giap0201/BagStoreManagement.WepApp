using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class ChiTietPhieuNhap
    {
        public int MaChiTietNhap { get; set; }
        public int MaPhieuNhap { get; set; }
        public int MaChiTietSP { get; set; }
        public int SoLuongNhap { get; set; }
        public decimal DonGia { get; set; }

        public PhieuNhapHang PhieuNhapHang { get; set; }
        public ChiTietSanPham ChiTietSanPham { get; set; }
    }
}