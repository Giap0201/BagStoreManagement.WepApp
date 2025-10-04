using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class AnhSanPham
    {
        public int MaAnh { get; set; }
        public int? MaSanPham { get; set; }
        public int? MaChiTietSanPham { get; set; }

        public string DuongDan { get; set; }
        public int ThuTuHienThi { get; set; }
        public bool HinhChinh { get; set; }

        public SanPham? SanPham { get; set; }
        public ChiTietSanPham? ChiTietSanPham { get; set; }
    }
}