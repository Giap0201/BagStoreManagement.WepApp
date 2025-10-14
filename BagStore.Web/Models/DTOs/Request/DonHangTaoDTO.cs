namespace BagStore.Web.Models.DTOs.Request
{
    public class DonHangTaoDTO
    {
        public int MaKH { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string PhuongThucThanhToan { get; set; }

        // Danh sách sản phẩm đặt mua
        public List<DonHangChiTietTaoDTO> ChiTietDonHangs { get; set; } = new();
    }
}
