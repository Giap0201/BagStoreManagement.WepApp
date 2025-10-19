namespace BagStore.Web.Models.ViewModels.SanPhams
{
    public class SanPhamDetailResponseDto
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string MoTaChiTiet { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public int MaLoaiTui { get; set; }
        public string TenLoaiTui { get; set; } // Join
        public int MaThuongHieu { get; set; }
        public string TenThuongHieu { get; set; } // Join
        public int MaChatLieu { get; set; }
        public string TenChatLieu { get; set; } // Join
        public DateTime NgayCapNhat { get; set; }
        public List<ChiTietSanPhamResponseDto> ChiTietSanPhams { get; set; } = new List<ChiTietSanPhamResponseDto>();
        public List<AnhSanPhamResponseDto> AnhSanPhams { get; set; } = new List<AnhSanPhamResponseDto>();
    }
}