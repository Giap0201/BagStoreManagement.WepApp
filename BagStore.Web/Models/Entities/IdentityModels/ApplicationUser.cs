using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        // Additional properties can be added here as needed
        public string? HoTen { get; set; }

        public DateTime NgayTaoTaiKhoan { get; set; }
        public DateTime? LastLogin { get; set; }

        // Navigation properties
        public KhachHangProfile? KhachHangProfile { get; set; }

        public NhanVienProfile? NhanVienProfile { get; set; }
    }
}