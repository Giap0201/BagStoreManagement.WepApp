using System.Collections.Generic;
using BagStore.Domain.Entities;

namespace BagStore.Web.Areas.Client.Models
{
    public class ProductDetailViewModel
    {
        public ChiTietSanPham Variant { get; set; } = null!;         // variant currently shown (MaChiTietSP)
        public SanPham? Product => Variant?.SanPham;
        public List<ChiTietSanPham> AllVariants { get; set; } = new();
        public List<AnhSanPham> Images { get; set; } = new();
    }
}
