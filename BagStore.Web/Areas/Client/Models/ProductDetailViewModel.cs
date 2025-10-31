using System.Collections.Generic;
using BagStore.Domain.Entities;

namespace BagStore.Web.Areas.Client.Models
{
    public class ProductDetailViewModel
    {
        public ChiTietSanPham Variant { get; set; }
        public List<ChiTietSanPham> AllVariants { get; set; } = new();
        public List<AnhSanPham> Images { get; set; } = new();
        public SanPham Product => Variant?.SanPham;
    }
}
