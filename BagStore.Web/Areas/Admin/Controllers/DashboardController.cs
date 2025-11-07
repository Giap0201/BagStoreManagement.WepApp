using BagStore.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Route("Admin/Dashboard")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var namHienTai = DateTime.Now.Year;
            var response = await _dashboardService.LayDuLieuDashboardAsync(namHienTai);
            if (response.Status == "error" || response.Data == null)
            {
                ViewBag.ErrorMessage = response.Message ?? "Không thể tải dữ liệu Dashboard";
                return View("Error");
            }
            return View(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetRevenueChart(int nam)
        {
            var response = await _dashboardService.LayDuLieuDashboardAsync(nam);
            if (response.Status == "error")
                return BadRequest(response);

            // Chỉ trả lại phần biểu đồ doanh thu để AJAX load
            return Json(response.Data.BieuDoDoanhThu);
        }
    }
}