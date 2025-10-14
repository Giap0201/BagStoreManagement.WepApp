namespace BagStore.Web.Models.ViewModels.SanPhams
{
    public class SanPhamListResponseDTO
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string TenLoaiTui { get; set; } // Join từ DanhMucLoaiTui
        public string TenThuongHieu { get; set; } // Join từ ThuongHieu
        public decimal GiaBanMin { get; set; } // Min từ ChiTietSanPham
        public int SoLuongTonTong { get; set; } // Sum từ ChiTietSanPham
        public DateTime NgayCapNhat { get; set; }
    }
}