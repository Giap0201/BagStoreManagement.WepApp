using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class PhieuNhapHang
    {
        public int MaPhieuNhap { get; set; }
        public int MaNCC { get; set; }
        public DateTime NgayNhap { get; set; }
        public decimal TongTienNhap { get; set; }

        public NhaCungCap NhaCungCap { get; set; }
        public ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
    }
}