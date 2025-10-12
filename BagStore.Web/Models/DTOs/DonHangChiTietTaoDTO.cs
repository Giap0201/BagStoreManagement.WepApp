namespace BagStore.Web.Models.DTOs
{
    public class DonHangChiTietTaoDTO
    {
        public int MaChiTietSP { get; set; }
        public string TenSP { get; set; }      // thêm tên sản phẩm
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
    }
}
