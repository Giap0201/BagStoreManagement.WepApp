using BagStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize] // ✅ Yêu cầu đăng nhập để xem giỏ hàng
    public class CartController : Controller
    {
        private readonly BagStoreDbContext _context;

        public CartController(BagStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // ✅ Lấy MaKH từ user hiện tại
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var khachHang = _context.KhachHangs.FirstOrDefault(k => k.ApplicationUserId == userId);
            if (khachHang == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserId = khachHang.MaKH;
            return View();
        }
    }
}
