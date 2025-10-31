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
        public int MaSP { get; set; }
        public string DuongDan { get; set; }
        public int ThuTuHienThi { get; set; }
        public bool LaHinhChinh { get; set; }

        // Quan hệ
        public SanPham SanPham { get; set; }
    }
}