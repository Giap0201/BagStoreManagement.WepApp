using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Client.Controllers
{
    public class CartController : Controller
    {
        [Area("Client")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
