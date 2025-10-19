namespace BagStore.Web.Models.DTOs.Request
{
    public class UpdateDonHangStatusRequest
    {
        public int MaDonHang { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }
}
