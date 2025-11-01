using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("Client/[controller]")]
    public class CartController : Controller
    {

        public IActionResult Index (int id)
        {
            ViewBag.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return View();
        }
    }
}
