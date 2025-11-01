using System.Collections.Generic;
using BagStore.Domain.Entities;

namespace BagStore.Web.Areas.Client.Models
{
    public class ShopFilterViewModel
    {
        // filter inputs
        public int? SizeId { get; set; }
        public int? ColorId { get; set; }
        public int? CategoryId { get; set; }

        // paging
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 9;
        public int TotalPages { get; set; }

        // data
        public List<SanPham> SanPhams { get; set; } = new List<SanPham>();

        // filter options
        public List<KichThuoc> Sizes { get; set; } = new();
        public List<MauSac> Colors { get; set; } = new();
        public List<DanhMucLoaiTui> Categories { get; set; } = new();
    }
}