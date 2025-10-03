using BagStore.Domain.Entities.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class DonHang
    {
        public int MaDonHang { get; set; }
        public string UserId { get; set; } = null!;

        public DateTime NgayDatHang { get; set; } = DateTime.Now;

        public decimal TongGiaTriHang { get; set; }
        public decimal GiamGiaTong { get; set; } = 0;
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } = null!;
        public string DiaChiGiaoHang { get; set; } = null!;
        public string PhuongThucThanhToan { get; set; } = null!;
        public DateTime? NgayHoanThanh { get; set; }
        public decimal PhiGiaoHang { get; set; } = 0;

        // FK Nhân viên xử lý đơn hàng
        public string? NhanVienXuLyId { get; set; }

        // Navigation properties
        public KhachHangProfile KhachHangProfile { get; set; } = null!;

        public NhanVienProfile? NhanVienXuLy { get; set; }

        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
        public ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
    }
}