using Microsoft.AspNetCore.Identity;

namespace BagStore.Web.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Nếu muốn mở rộng thông tin cá nhân
        public string FullName { get; set; }

        public DateTime? NgaySinh { get; set; }

        // Quan hệ với KhachHang nếu là Customer
        public KhachHang KhachHang { get; set; }

        // Quan hệ với NhanVienProfile nếu là Admin/Staff
        public NhanVienProfile NhanVienProfile { get; set; }
    }
}