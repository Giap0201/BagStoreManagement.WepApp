using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("Client/[controller]")]
    public class CartController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id:int}")]
        public IActionResult getCart(int id)
        {
            Console.WriteLine($"📦 [CartController] userId nhận được: {id}");
            ViewBag.UserId = id;
            return View();
        }
    }
}
