using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class MauSac
    {
        public int MaMauSac { get; set; }
        public string TenMauSac { get; set; }
        public ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();
    }
}