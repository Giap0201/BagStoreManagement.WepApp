using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class NhaCungCap
    {
        public int MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }

        public ICollection<PhieuNhapHang> PhieuNhapHangs { get; set; }
    }
}