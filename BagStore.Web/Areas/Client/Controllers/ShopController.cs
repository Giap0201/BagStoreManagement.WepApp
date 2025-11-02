using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BagStore.Domain.Entities;
using BagStore.Data; // đổi nếu DbContext ở namespace khác
using BagStore.Web.Areas.Client.Models;

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
        public IActionResult Index(int? sizeId, int? colorId, int? categoryId, int? loaiTuiId, int? maThuongHieu, int? maChatLieu, int page = 1)
        {
            // Support both categoryId (old) and loaiTuiId (from DanhMucLoaiTui component)
            var finalCategoryId = categoryId ?? loaiTuiId;
            var vm = BuildFilterViewModel(sizeId, colorId, finalCategoryId, page);
            // Pass filter values to view for initialization
            ViewBag.MaThuongHieu = maThuongHieu;
            ViewBag.MaChatLieu = maChatLieu;
            ViewBag.LoaiTuiId = finalCategoryId;
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

        // GET: /Client/Shop/Detail/{maSP}
        public IActionResult Detail(int id) // id = MaSP
        {
            var sp = _context.SanPhams
                .Include(s => s.DanhMucLoaiTui)
                .Include(s => s.ThuongHieu)
                .Include(s => s.ChatLieu)
                .Include(s => s.AnhSanPhams)
                .Include(s => s.ChiTietSanPhams)
                    .ThenInclude(ct => ct.KichThuoc)
                .Include(s => s.ChiTietSanPhams)
                    .ThenInclude(ct => ct.MauSac)
                .FirstOrDefault(s => s.MaSP == id);

            if (sp == null) return NotFound();

            var firstVariant = sp.ChiTietSanPhams.FirstOrDefault();
            var vm = new ProductDetailViewModel
            {
                Variant = firstVariant ?? new ChiTietSanPham { SanPham = sp },
                AllVariants = sp.ChiTietSanPhams.ToList(),
                Images = sp.AnhSanPhams.OrderBy(a => a.ThuTuHienThi).ToList()
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

        // ✅ API lấy MaKH của user hiện tại (dùng cho modal thêm vào giỏ)
        // GET: /Client/Shop/GetMaKH
        public IActionResult GetMaKH()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { maKH = (int?)null });
            }

            var khachHang = _context.KhachHangs.FirstOrDefault(k => k.ApplicationUserId == userId);
            if (khachHang == null)
            {
                return Json(new { maKH = (int?)null });
            }

            return Json(new { maKH = khachHang.MaKH });
        }

        // ✅ API lấy danh sách màu sắc và kích thước của sản phẩm (cho popup)
        // GET: /Client/Shop/GetProductOptions?maSP=...
        public IActionResult GetProductOptions(int maSP)
        {
            var sanPham = _context.SanPhams
                .Include(sp => sp.ChiTietSanPhams)
                    .ThenInclude(ct => ct.KichThuoc)
                .Include(sp => sp.ChiTietSanPhams)
                    .ThenInclude(ct => ct.MauSac)
                .Include(sp => sp.AnhSanPhams)
                .FirstOrDefault(sp => sp.MaSP == maSP);

            if (sanPham == null)
                return NotFound(new { message = "Không tìm thấy sản phẩm" });

            // Lấy danh sách màu sắc và kích thước duy nhất
            var mauSacs = sanPham.ChiTietSanPhams
                .Select(ct => ct.MauSac)
                .Distinct()
                .Select(m => new { m.MaMauSac, m.TenMauSac })
                .ToList();

            var kichThuocs = sanPham.ChiTietSanPhams
                .Select(ct => ct.KichThuoc)
                .Distinct()
                .Select(k => new { k.MaKichThuoc, k.TenKichThuoc })
                .ToList();

            var anhChinh = sanPham.AnhSanPhams?.FirstOrDefault(a => a.LaHinhChinh)?.DuongDan;

            return Json(new
            {
                maSP = sanPham.MaSP,
                tenSP = sanPham.TenSP,
                anhChinh = anhChinh,
                mauSacs = mauSacs,
                kichThuocs = kichThuocs
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
