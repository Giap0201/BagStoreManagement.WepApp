using BagStore.Domain.Entities.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class PhieuNhapHang
    {
        public int MaPhieuNhap { get; set; }
        public int MaNhaCungCap { get; set; }
        public DateTime NgayNhap { get; set; }
        public string NhanVienTaoId { get; set; }
        public decimal TongTien { get; set; }

        // Navigation Properties
        public NhaCungCap NhaCungCap { get; set; }

        public NhanVienProfile NhanVienTao { get; set; }
        public ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();
    }
}