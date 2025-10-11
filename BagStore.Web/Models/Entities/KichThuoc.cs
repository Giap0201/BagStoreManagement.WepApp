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
        public int MaKichThuoc { get; set; }
        public string TenKichThuoc { get; set; }
        public decimal? ChieuDai { get; set; }
        public decimal? ChieuRong { get; set; }
        public decimal? ChieuCao { get; set; }

        public ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; }
    }
}