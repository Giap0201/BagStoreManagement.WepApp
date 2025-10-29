using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DonHangController : Controller
    {
        private readonly IHttpClientFactory _httpFactory;

        public DonHangController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }
        // GET: /Admin/DonHang
        public IActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return NotFound();
            }

            return View();
        }

        // (Optional) If you want server side to call API and pass data to view, inject IHttpClientFactory
    }
}
