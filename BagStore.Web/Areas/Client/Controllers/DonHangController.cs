using BagStore.Web.Models.DTOs.Request;
using BagStore.Web.Models.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Area("Client")]
    public class DonHangController : Controller
    {
        private readonly IHttpClientFactory _httpFactory;

        public DonHangController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        // GET: /Client/Orders/Checkout
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        // GET: /Client/Orders/Index
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.MaKH = 2;
            return View(); // view sẽ load data bằng AJAX
        }

        // (Optional) Nếu muốn server-side lấy data và render view:
        // public async Task<IActionResult> Index()
        // {
        //     var client = _httpFactory.CreateClient("BagStoreApi");
        //     var userId = GetCurrentUserId(); // implement lấy id từ session/claims
        //     var res = await client.GetFromJsonAsync<List<DonHangPhanHoiDTO>>($"api/DonHangApi/KhachHang/{userId}");
        //     return View(res);
        // }
    }
}