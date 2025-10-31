using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class SanPham
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string MoTaChiTiet { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public int MaLoaiTui { get; set; }
        public int MaThuongHieu { get; set; }
        public int MaChatLieu { get; set; }
        public DateTime NgayCapNhat { get; set; }

        // Quan hệ
        public DanhMucLoaiTui DanhMucLoaiTui { get; set; }

        public ThuongHieu ThuongHieu { get; set; }
        public ChatLieu ChatLieu { get; set; }

        public ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; }
        public ICollection<AnhSanPham> AnhSanPhams { get; set; }
        public ICollection<DanhGia> DanhGias { get; set; }
    }
}