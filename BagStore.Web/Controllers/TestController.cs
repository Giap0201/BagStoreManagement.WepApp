using Microsoft.AspNetCore.Mvc;

namespace BagStoreManagement.WepApp.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
