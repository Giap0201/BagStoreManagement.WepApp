using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class KhoHang
    {
        public int MaKho { get; set; }
        public string TenKho { get; set; }
        public string? DiaChi { get; set; }

        // Navigation Properties
        public ICollection<TonKho> TonKhos { get; set; } = new List<TonKho>();
    }
}