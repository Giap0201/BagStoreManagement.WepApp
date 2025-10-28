namespace BagStore.Web.Models.DTOs.Requests
{
    public class CreateDonHangRequest
    {
        public int MaKhachHang { get; set; }
        public string DiaChiGiaoHang { get; set; } = string.Empty;
        public string PhuongThucThanhToan { get; set; } = "COD";

        public List<CreateDonHangChiTietRequest> ChiTietDonHang { get; set; } = new();
    }
}
