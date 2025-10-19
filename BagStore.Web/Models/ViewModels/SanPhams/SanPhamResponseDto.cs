namespace BagStore.Web.Models.ViewModels.SanPhams
{
    public class SanPhamResponseDto
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string MoTaChiTiet { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }

        public int MaLoaiTui { get; set; }
        public string TenLoaiTui { get; set; }

        public int MaThuongHieu { get; set; }
        public string TenThuongHieu { get; set; }

        public int MaChatLieu { get; set; }
        public string TenChatLieu { get; set; }

        public string AnhChinh { get; set; }  // URL ảnh chính
        public DateTime NgayCapNhap { get; set; }
    }
}