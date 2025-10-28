namespace BagStore.Web.Models.DTOs.Requests
{
    public class CreateDonHangChiTietRequest
    {
        public int MaChiTietSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
    }
}
