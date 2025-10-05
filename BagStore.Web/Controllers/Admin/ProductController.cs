using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Admin
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
