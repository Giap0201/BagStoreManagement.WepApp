using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class DanhMucLoaiTui
    {
        public int MaLoaiTui { get; set; }
        public string TenLoaiTui { get; set; }
        public string MoTa { get; set; }

        // Quan hệ 1 - N với SanPham
        public ICollection<SanPham> SanPhams { get; set; }
    }
}