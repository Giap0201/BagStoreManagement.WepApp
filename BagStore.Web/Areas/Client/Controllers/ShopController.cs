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
            try
            {
                var q = _context.ChiTietSanPhams.AsQueryable().Where(ct => ct.MaSP == maSp);

                if (sizeId.HasValue)
                    q = q.Where(ct => ct.MaKichThuoc == sizeId.Value);

                if (colorId.HasValue)
                    q = q.Where(ct => ct.MaMauSac == colorId.Value);

                var variant = q.FirstOrDefault();

                if (variant == null)
                    return Json(new { status = "error", message = "Không tìm thấy biến thể sản phẩm" });

                // trả về JSON nhỏ gọn (dùng camelCase để nhất quán với JavaScript)
                return Json(new
                {
                    maChiTietSP = variant.MaChiTietSP,
                    MaChiTietSP = variant.MaChiTietSP, // Giữ cả 2 để tương thích
                    giaBan = variant.GiaBan,
                    GiaBan = variant.GiaBan,
                    soLuongTon = variant.SoLuongTon,
                    SoLuongTon = variant.SoLuongTon
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = "Lỗi server", error = ex.Message });
            }
        }

        // ✅ API lấy MaKH của user hiện tại (dùng cho modal thêm vào giỏ)
        // GET: /Client/Shop/GetMaKH
        public IActionResult GetMaKH()
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = "Lỗi server", error = ex.Message });
            }
        }

        // ✅ API lấy danh sách màu sắc và kích thước của sản phẩm (cho popup)
        // GET: /Client/Shop/GetProductOptions?maSP=...
        public IActionResult GetProductOptions(int maSP)
        {
            try
            {
                var sanPham = _context.SanPhams
                    .AsNoTracking() // ✅ Tránh circular reference khi serialize
                    .Include(sp => sp.ChiTietSanPhams)
                        .ThenInclude(ct => ct.KichThuoc)
                    .Include(sp => sp.ChiTietSanPhams)
                        .ThenInclude(ct => ct.MauSac)
                    .Include(sp => sp.AnhSanPhams)
                    .FirstOrDefault(sp => sp.MaSP == maSP);

                if (sanPham == null)
                    return Json(new { status = "error", message = "Không tìm thấy sản phẩm" });

                // ✅ Kiểm tra null cho ChiTietSanPhams và lấy màu sắc/kích thước
                var mauSacsList = new List<Dictionary<string, object>>();
                if (sanPham.ChiTietSanPhams != null && sanPham.ChiTietSanPhams.Any())
                {
                    mauSacsList = sanPham.ChiTietSanPhams
                        .Where(ct => ct != null && ct.MauSac != null)
                        .Select(ct => new { maMauSac = ct.MauSac.MaMauSac, tenMauSac = ct.MauSac.TenMauSac ?? "" })
                        .GroupBy(m => m.maMauSac)
                        .Select(g => new Dictionary<string, object>
                        {
                            { "maMauSac", g.Key },
                            { "tenMauSac", g.First().tenMauSac }
                        })
                        .ToList();
                }

                var kichThuocsList = new List<Dictionary<string, object>>();
                if (sanPham.ChiTietSanPhams != null && sanPham.ChiTietSanPhams.Any())
                {
                    kichThuocsList = sanPham.ChiTietSanPhams
                        .Where(ct => ct != null && ct.KichThuoc != null)
                        .Select(ct => new { maKichThuoc = ct.KichThuoc.MaKichThuoc, tenKichThuoc = ct.KichThuoc.TenKichThuoc ?? "" })
                        .GroupBy(k => k.maKichThuoc)
                        .Select(g => new Dictionary<string, object>
                        {
                            { "maKichThuoc", g.Key },
                            { "tenKichThuoc", g.First().tenKichThuoc }
                        })
                        .ToList();
                }

                // Lấy ảnh chính hoặc ảnh đầu tiên
                string anhChinh = null;
                if (sanPham.AnhSanPhams != null && sanPham.AnhSanPhams.Any())
                {
                    var anh = sanPham.AnhSanPhams.FirstOrDefault(a => a.LaHinhChinh)
                        ?? sanPham.AnhSanPhams.FirstOrDefault();
                    anhChinh = anh?.DuongDan;
                }

                return Json(new
                {
                    maSP = sanPham.MaSP,
                    tenSP = sanPham.TenSP ?? "",
                    anhChinh = anhChinh ?? "",
                    mauSacs = mauSacsList,
                    kichThuocs = kichThuocsList
                });
            }
            catch (Exception ex)
            {
                // ✅ Log lỗi chi tiết
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Lỗi server",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
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
