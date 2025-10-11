namespace BagStore.Web.Models.Entities
{
    public class NhanVienProfile
    {
        public int Id { get; set; }
        public string MaNhanVien { get; set; }
        public string ChucVu { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }

        // Liên kết Identity
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}