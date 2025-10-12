namespace BagStore.Web.Models.DTOs
{
    public class SanPhamCreateDto
    {
        public string TenSP { get; set; }
        public string MoTaChiTiet { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public int MaLoaiTui { get; set; }
        public int MaThuongHieu { get; set; }
        public int MaChatLieu { get; set; }
        public DateTime NgayCapNhat { get; set; }
    }
}