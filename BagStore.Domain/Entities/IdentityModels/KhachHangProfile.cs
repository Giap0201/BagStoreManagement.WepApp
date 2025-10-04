using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities.IdentityModels
{
    public class KhachHangProfile
    {
        public string UserId { get; set; }
        public string? DiaChiMacDinh { get; set; }

        // Navigation Properties
        public ApplicationUser User { get; set; }

        public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
        public ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();
        public ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
    }
}