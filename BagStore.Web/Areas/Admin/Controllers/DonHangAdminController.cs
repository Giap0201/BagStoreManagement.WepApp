using BagStore.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DonHangAdminController : Controller
    {
        private readonly IDonHangRepository _donHangRepo;

        public DonHangAdminController(IDonHangRepository donHangRepo)
        {
            _donHangRepo = donHangRepo;
        }

        // GET: /Admin/Orders
        public async Task<IActionResult> Orders()
        {
            var donHangs = await _donHangRepo.LayTatCaDonHangAsync();
            return View(donHangs);
        }

        // POST: /Admin/Orders/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int maDonHang, string trangThai)
        {
            var donHang = await _donHangRepo.LayTheoIdAsync(maDonHang);
            if (donHang == null)
            {
                TempData["Error"] = "Đơn hàng không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            donHang.TrangThai = trangThai;
            await _donHangRepo.CapNhatAsync(donHang);
            await _donHangRepo.LuuAsync();

            TempData["Success"] = "Cập nhật trạng thái thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
