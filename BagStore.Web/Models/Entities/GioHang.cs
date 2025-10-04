using BagStore.Domain.Entities.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class GioHang
    {
        public int MaGioHang { get; set; }
        public string UserId { get; set; }
        public int MaChiTietSanPham { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayThem { get; set; }
        public string TrangThai { get; set; }

        public ChiTietSanPham ChiTietSanPham { get; set; }
        public KhachHangProfile KhachHangProfile { get; set; }
    }
}