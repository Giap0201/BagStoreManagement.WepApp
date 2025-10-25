using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        public IActionResult MauSac()
        {
            return View();
        }

        public IActionResult ThuongHieu()
        {
            return View();
        }

        public IActionResult KichThuoc()
        {
            return View();
        }

        public IActionResult SanPham()
        {
            return View();
        }

        [HttpGet("{x}")]
        public IActionResult Test(int x)
        {
            return View();
        }
    }
}