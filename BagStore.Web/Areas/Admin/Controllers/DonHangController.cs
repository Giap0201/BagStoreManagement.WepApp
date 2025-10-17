using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DonHangController : Controller
    {
        // GET: /Admin/Orders
        public IActionResult Index()
        {
            return View();
        }

        // (Optional) If you want server side to call API and pass data to view, inject IHttpClientFactory
    }
}
