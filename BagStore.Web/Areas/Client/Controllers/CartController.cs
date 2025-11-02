using BagStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index (int id)
        {
            ViewBag.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return View();
        }
    }
}