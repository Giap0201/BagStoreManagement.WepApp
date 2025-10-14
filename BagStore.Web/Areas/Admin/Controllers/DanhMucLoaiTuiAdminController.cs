using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DanhMucLoaiTuiAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}