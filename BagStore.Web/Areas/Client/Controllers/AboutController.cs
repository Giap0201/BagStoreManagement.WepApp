using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Client.Controllers
{
    public class AboutController : Controller
    {
        [Area("Client")]
        public IActionResult Index()
        {
            return View();
        }
    }
}