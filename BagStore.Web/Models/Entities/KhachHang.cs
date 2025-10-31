using BagStore.Domain.Entities;

namespace BagStore.Web.Models.Entities
{
    public class KhachHang
    {
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChiMacDinh { get; set; }
        public DateTime NgayDangKy { get; set; }

        // Mới: Liên kết với Identity
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<GioHang> GioHangs { get; set; }
        public ICollection<DonHang> DonHangs { get; set; }
        public ICollection<DanhGia> DanhGias { get; set; }
    }
}