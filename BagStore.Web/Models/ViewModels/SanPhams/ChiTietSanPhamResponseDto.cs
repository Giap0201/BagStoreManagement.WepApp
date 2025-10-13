namespace BagStore.Web.Models.ViewModels.SanPhams
{
    public class ChiTietSanPhamResponseDto
    {
        public int MaChiTietSP { get; set; }
        public int MaKichThuoc { get; set; }
        public string TenKichThuoc { get; set; } // Join
        public int MaMauSac { get; set; }
        public string TenMauSac { get; set; } // Join
        public int SoLuongTon { get; set; }
        public decimal GiaBan { get; set; }
    }
}