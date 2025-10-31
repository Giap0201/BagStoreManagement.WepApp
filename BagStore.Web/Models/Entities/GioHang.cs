using BagStore.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class GioHang
    {
        public int MaSP_GH { get; set; }
        public int? MaKH { get; set; }
        public int MaChiTietSP { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayThem { get; set; }

        // Quan hệ
        public KhachHang KhachHang { get; set; }

        public ChiTietSanPham ChiTietSanPham { get; set; }
    }
}