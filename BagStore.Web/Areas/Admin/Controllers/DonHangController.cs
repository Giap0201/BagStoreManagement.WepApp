using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DonHangController : Controller
    {
        private readonly IHttpClientFactory _httpFactory;

        public DonHangController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        // GET: /Admin/Orders
        [Route("Admin/DonHang")]
        [Route("Admin/DonHang/Index")]
        public IActionResult Index()
        {
            return View();
        }

        // (Optional) If you want server side to call API and pass data to view, inject IHttpClientFactory
    }
}