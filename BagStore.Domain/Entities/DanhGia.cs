using BagStore.Domain.Entities.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class DanhGia
    {
        public int MaDanhGia { get; set; }
        public string UserId { get; set; }
        public int MaSanPham { get; set; }
        public int Diem { get; set; } // Assuming a rating scale, e.g., 1-5
        public string? BinhLuan { get; set; }
        public DateTime NgayDanhGia { get; set; }

        // Navigation Properties
        public SanPham SanPham { get; set; }

        public KhachHangProfile KhachHangProfile { get; set; }
    }
}