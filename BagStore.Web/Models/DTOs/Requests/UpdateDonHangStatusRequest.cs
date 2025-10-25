namespace BagStore.Web.Models.DTOs.Requests
{
    public class UpdateDonHangStatusRequest
    {
        public int MaDonHang { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }
}
