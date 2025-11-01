using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Area("Client")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
