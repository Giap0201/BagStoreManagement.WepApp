using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class KichThuoc
    {
        [Key]
        public int MaKichThuoc { get; set; }

        [Required]
        [MaxLength(100)]
        public string TenKichThuoc { get; set; }

        public decimal ChieuDai { get; set; }
        public decimal ChieuRong { get; set; }
        public decimal ChieuCao { get; set; }

        public ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();
    }
}