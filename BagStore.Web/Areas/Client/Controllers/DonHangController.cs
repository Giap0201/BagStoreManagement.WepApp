using BagStore.Data;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Authorize]
    [Area("Client")]
    [Route("Client/[controller]/[action]")]
    public class DonHangController : Controller
    {
        private readonly IDonHangService _donHangService;
        private readonly BagStoreDbContext _context;

        public DonHangController(BagStoreDbContext context,IDonHangService donHangService)
        {
            _context = context;
            _donHangService = donHangService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1️⃣ Lấy Id user đang đăng nhập (từ Claims Identity)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;

            // 4️⃣ Trả dữ liệu ra View
            return View();
        }

        [HttpGet]
        public IActionResult Checkout(int? maChiTietSP = null, int? soLuong = null, int? maSanPham = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var khachHang = _context.KhachHangs.FirstOrDefault(kh => kh.ApplicationUserId == userId);
            int maKH = khachHang.MaKH;
            ViewBag.UserId = maKH;


            if (maChiTietSP.HasValue && soLuong.HasValue && maSanPham.HasValue)
            {
                // ✅ Trường hợp "Mua ngay"
                ViewBag.IsBuyNow = true;
                ViewBag.MaChiTietSP = maChiTietSP.Value;
                ViewBag.SoLuong = soLuong.Value;
                ViewBag.MaSanPham = maSanPham.Value;
            }
            else
            {
                // ✅ Trường hợp "Mua từ giỏ hàng"
                ViewBag.IsBuyNow = false;
            }

            return View();
        }

        [HttpGet("{maDH:int}")]
        public IActionResult Details(int maDH)
        {
            // Không cần truyền model, fetch từ API
            ViewData["MaDonHang"] = maDH;
            return View();
        }

    }
}
