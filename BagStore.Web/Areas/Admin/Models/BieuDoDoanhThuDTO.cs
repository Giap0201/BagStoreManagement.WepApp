namespace BagStore.Web.Areas.Admin.Models
{
    public class BieuDoDoanhThuDTO
    {
        public List<string> NhanThang { get; set; } = new List<string>();
        public List<decimal> GiaTriDoanhThu { get; set; } = new List<decimal>();
    }
}