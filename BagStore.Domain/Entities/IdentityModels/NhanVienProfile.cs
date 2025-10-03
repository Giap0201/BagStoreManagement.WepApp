using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities.IdentityModels
{
    public class NhanVienProfile
    {
        public string UserId { get; set; } = null!;
        public string? ChucVu { get; set; }

        public ApplicationUser ApplicationUser { get; set; } = null!;

        // Mối liên hệ nghiệp vụ (Quản lý Kho, Sản phẩm, Đơn hàng, Thanh toán)
        public ICollection<PhieuNhapHang> PhieuNhapHangs { get; set; } = new List<PhieuNhapHang>();

        public ICollection<DonHang> DonHangDaXuLys { get; set; } = new List<DonHang>();
        public ICollection<SanPham> SanPhamDaCapNhats { get; set; } = new List<SanPham>();
        public ICollection<ThanhToan> ThanhToanDaXacNhans { get; set; } = new List<ThanhToan>();
    }
}