using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class ThuongHieu
    {
        public int MaThuongHieu { get; set; }
        public string TenThuongHieu { get; set; }
        public string QuocGia { get; set; }

        public ICollection<SanPham> SanPhams { get; set; }
    }
}