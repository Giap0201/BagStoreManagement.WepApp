using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class NhaCungCap
    {
        public int MaNhaCungCap { get; set; }
        public string TenNhaCungCap { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }

        // Navigation Properties
        public ICollection<PhieuNhapHang> PhieuNhapHangs { get; set; } = new List<PhieuNhapHang>();
    }
}