using BagStore.Domain.Entities.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class ThanhToan
    {
        public int MaThanhToan { get; set; }
        public int MaDonHang { get; set; }

        public decimal SoTien { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public string TrangThai { get; set; } = null!;

        // FK Nhân viên xác nhận thanh toán
        public string? NhanVienXacNhanId { get; set; }

        // Navigation property
        public DonHang DonHang { get; set; } = null!;

        public NhanVienProfile? NhanVienXacNhan { get; set; }
    }
}