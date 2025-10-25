using BagStore.Services.Interfaces;
using BagStore.Web.Models.ViewModels;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BienTheController : Controller
    {
        private readonly ISanPhamService _service;
        private readonly IThuongHieuService _thuongHieuService;
        private readonly IChatLieuService _chatLieuService;
        private readonly IDanhMucLoaiTuiService _danhMucLoaiTuiService;
        private readonly IChiTietSanPhamService _chiTietService;
        private readonly IAnhSanPhamService _anhService;

        public BienTheController(
         ISanPhamService service,
         IThuongHieuService thuongHieuService,
         IChatLieuService chatLieuService,
         IDanhMucLoaiTuiService danhMucLoaiTuiService,
         IChiTietSanPhamService chiTietService,
         IAnhSanPhamService anhService)
        {
            _service = service;
            _chatLieuService = chatLieuService;
            _thuongHieuService = thuongHieuService;
            _danhMucLoaiTuiService = danhMucLoaiTuiService;
            _chiTietService = chiTietService;
            _anhService = anhService;
        }

        public async Task<IActionResult> IndexAsync(int id)
        {
            var sanPhamResponse = await _service.GetByIdAsync(id);
            if (sanPhamResponse.Status != "success" || sanPhamResponse.Data == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            var chiTietResponse = await _chiTietService.GetBySanPhamIdAsync(id);
            var anhResponse = await _anhService.GetBySanPhamIdAsync(id);

            var model = new SanPhamDetailViewModel
            {
                SanPham = sanPhamResponse.Data,
                ChiTietSanPhams = chiTietResponse.Data ?? new List<ChiTietSanPhamResponseDto>(),
                AnhSanPhams = anhResponse.Data ?? new List<AnhSanPhamResponseDto>()
            };

            return View(model);
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}