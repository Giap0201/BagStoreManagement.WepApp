using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        private readonly ISanPhamService _sanPhamService;
        private readonly IDanhMucLoaiTuiRepository _repoLoaiTui;
        private readonly IThuongHieuRepository _repoThuongHieu;
        private readonly IChatLieuRepository _repoChatLieu;

        public SanPhamController(
            ISanPhamService sanPhamService,
            IDanhMucLoaiTuiRepository repoLoaiTui,
            IThuongHieuRepository repoThuongHieu,
            IChatLieuRepository repoChatLieu)
        {
            _sanPhamService = sanPhamService;
            _repoLoaiTui = repoLoaiTui;
            _repoThuongHieu = repoThuongHieu;
            _repoChatLieu = repoChatLieu;
        }

        // Hiển thị danh sách sản phẩm
        public IActionResult Index()
        {
            return View();
        }

        // Hiển thị trang thêm mới
        public IActionResult Create()
        {
            return View(new SanPhamRequestDto());
        }

        // Hiển thị trang chỉnh sửa
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _sanPhamService.GetByIdAsync(id);
            if (response.Status == "error")
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            var dto = new SanPhamRequestDto
            {
                MaSanPham = response.Data.MaSP,
                TenSP = response.Data.TenSP,
                MoTaChiTiet = response.Data.MoTaChiTiet,
                MetaTitle = response.Data.MetaTitle,
                MetaDescription = response.Data.MetaDescription,
                MaLoaiTui = response.Data.MaLoaiTui,
                MaThuongHieu = response.Data.MaThuongHieu,
                MaChatLieu = response.Data.MaChatLieu
            };
            ViewBag.CurrentImage = response.Data.AnhChinh;
            return View(dto);
        }
    }
}