using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DanhMucLoaiTui()
        {
            return View();
        }

        public IActionResult ChatLieu()
        {
            return View();
        }
    }
}