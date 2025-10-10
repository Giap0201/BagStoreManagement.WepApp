using BagStore.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class DonHang
    {
        public int MaDonHang { get; set; }
        public int MaKH { get; set; }
        public DateTime NgayDatHang { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public decimal PhiGiaoHang { get; set; }
        public string TrangThaiThanhToan { get; set; }
        public DateTime? NgayThanhToan { get; set; }

        // Quan hệ
        public KhachHang KhachHang { get; set; }

        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}