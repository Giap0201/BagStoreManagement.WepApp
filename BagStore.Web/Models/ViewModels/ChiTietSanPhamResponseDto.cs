namespace BagStore.Web.Models.ViewModels
{
    public class ChiTietSanPhamResponseDto
    {
        public int MaSP { get; set; } // Join
        public int MaChiTietSP { get; set; }
        public string TenSanPham { get; set; } // Join
        public int MaKichThuoc { get; set; }
        public string TenKichThuoc { get; set; } // Join
        public int MaMauSac { get; set; }
        public string TenMauSac { get; set; } // Join
        public int SoLuongTon { get; set; }
        public decimal GiaBan { get; set; }
        public DateTime NgayTao { get; set; }

        public string duongDanAnh { get; set; } // Join
    }
}