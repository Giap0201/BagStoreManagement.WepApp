using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class TonKho
    {
        public int MaTonKho { get; set; }
        public int MaChiTietSanPham { get; set; }
        public int MaKho { get; set; }
        public int SoLuongTon { get; set; }
        public DateTime NgayCapNhat { get; set; }

        // Navigation Properties
        public ChiTietSanPham ChiTietSanPham { get; set; }

        public KhoHang KhoHang { get; set; }
    }
}