using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("Client/[controller]")]
    public class CartController : Controller
    {

        [HttpGet("{id:int}")]
        public IActionResult Index (int id)
        {
            ViewBag.UserId = id;
            return View();
        }
    }
}
