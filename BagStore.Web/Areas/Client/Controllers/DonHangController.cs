using BagStore.Web.Models.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize]
    public class DonHangController : Controller
    {
        private readonly IHttpClientFactory _httpFactory;

        public DonHangController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
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
        public IActionResult Checkout(int? maChiTietSP = null, int? soLuong = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;

            if (maChiTietSP.HasValue && soLuong.HasValue)
            {
                // Mua ngay
                ViewBag.BuyNow = true;
                ViewBag.MaChiTietSP = maChiTietSP.Value;
                ViewBag.SoLuong = soLuong.Value;
            }
            else
            {
                // Từ giỏ hàng
                ViewBag.BuyNow = true;
                ViewBag.MaChiTietSP = 3;
                ViewBag.SoLuong = 1;
            }

            return View();
        }
    }
}
