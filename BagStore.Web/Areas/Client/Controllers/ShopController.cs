using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BagStore.Domain.Entities;
using BagStore.Data; // đổi nếu DbContext ở namespace khác
using BagStore.Web.Areas.Client.Models;
using BagStore.Data;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Area("Client")]
    public class ShopController : Controller
    {
        private readonly BagStoreDbContext _context;

        public ShopController(BagStoreDbContext context)
        {
            _context = context;
        }

        // GET: /Client/Shop
        public IActionResult Index(int? sizeId, int? colorId, int? categoryId, int page = 1)
        {
            var vm = BuildFilterViewModel(sizeId, colorId, categoryId, page);
            return View(vm);
        }

        // AJAX endpoint: trả về partial HTML (product list)
        // GET: /Client/Shop/Filter
        public IActionResult Filter(int? sizeId, int? colorId, int? categoryId, int page = 1)
        {
            var vm = BuildFilterViewModel(sizeId, colorId, categoryId, page);
            // trả về partial view HTML để AJAX cập nhật
            return PartialView("~/Areas/Client/Views/Shop/_ProductList.cshtml", vm);
        }

        // GET: /Client/Shop/Detail/{maChiTietSP}
        public IActionResult Detail(int id) // 'id' = MaChiTietSP
        {
            var variant = _context.ChiTietSanPhams
                .Include(ct => ct.SanPham)
                    .ThenInclude(s => s.AnhSanPhams)
                .Include(ct => ct.KichThuoc)
                .Include(ct => ct.MauSac)
                .FirstOrDefault(ct => ct.MaChiTietSP == id);

            if (variant == null) return NotFound();

            var allVariants = _context.ChiTietSanPhams
                .Include(ct => ct.KichThuoc)
                .Include(ct => ct.MauSac)
                .Where(ct => ct.MaSP == variant.MaSP)
                .ToList();

            var images = variant.SanPham.AnhSanPhams.OrderBy(a => a.ThuTuHienThi).ToList();

            var vm = new ProductDetailViewModel
            {
                Variant = variant,
                AllVariants = allVariants,
                Images = images
            };

            return View(vm);
        }

        // API giúp tìm biến thể theo MaSP + MaKichThuoc + MaMauSac
        // GET: /Client/Shop/GetVariant?maSp=...&sizeId=...&colorId=...
        public IActionResult GetVariant(int maSp, int? sizeId, int? colorId)
        {
            var q = _context.ChiTietSanPhams.AsQueryable().Where(ct => ct.MaSP == maSp);

            if (sizeId.HasValue)
                q = q.Where(ct => ct.MaKichThuoc == sizeId.Value);

            if (colorId.HasValue)
                q = q.Where(ct => ct.MaMauSac == colorId.Value);

            var variant = q.FirstOrDefault();

            if (variant == null) return NotFound();

            // trả về JSON nhỏ gọn
            return Json(new
            {
                MaChiTietSP = variant.MaChiTietSP,
                GiaBan = variant.GiaBan,
                SoLuongTon = variant.SoLuongTon
            });
        }

        // ------- Helper -------
        private ShopFilterViewModel BuildFilterViewModel(int? sizeId, int? colorId, int? categoryId, int page)
        {
            int pageSize = 9;

            // base query (include liên quan cần thiết)
            var query = _context.SanPhams
                .Include(s => s.AnhSanPhams)
                .Include(s => s.ChiTietSanPhams)
                    .ThenInclude(ct => ct.KichThuoc)
                .Include(s => s.ChiTietSanPhams)
                    .ThenInclude(ct => ct.MauSac)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(s => s.MaLoaiTui == categoryId.Value);

            if (sizeId.HasValue)
                query = query.Where(s => s.ChiTietSanPhams.Any(ct => ct.MaKichThuoc == sizeId.Value));

            if (colorId.HasValue)
                query = query.Where(s => s.ChiTietSanPhams.Any(ct => ct.MaMauSac == colorId.Value));

            int total = query.Count();

            var paged = query
                .OrderByDescending(s => s.NgayCapNhat)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var vm = new ShopFilterViewModel
            {
                SanPhams = paged,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize),
                SizeId = sizeId,
                ColorId = colorId,
                CategoryId = categoryId,
                Sizes = _context.KichThuocs.ToList(),
                Colors = _context.MauSacs.ToList(),
                Categories = _context.DanhMucLoaiTuis.ToList()
            };

            return vm;
        }
    }
}
