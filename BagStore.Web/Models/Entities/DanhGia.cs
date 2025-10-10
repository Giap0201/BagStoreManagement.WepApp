using BagStore.Web.Models.Entities;

namespace BagStore.Domain.Entities
{
    public class DanhGia
    {
        public int MaDanhGia { get; set; }
        public int MaKH { get; set; }
        public int MaSP { get; set; }
        public int Diem { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayDanhGia { get; set; }

        public KhachHang KhachHang { get; set; }
        public SanPham SanPham { get; set; }
    }
}