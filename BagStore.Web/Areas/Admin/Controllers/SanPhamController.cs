using BagStore.Services.Interfaces;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Models.ViewModels;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        private readonly ISanPhamService _service;
        private readonly IThuongHieuService _thuongHieuService;
        private readonly IChatLieuService _chatLieuService;
        private readonly IDanhMucLoaiTuiService _danhMucLoaiTuiService;
        private readonly IChiTietSanPhamService _chiTietService;
        private readonly IAnhSanPhamService _anhService;

        public SanPhamController(
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

        // Hiển thị danh sách sản phẩm (ban đầu)
        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllAsync();
            var model = response.Data ?? new List<SanPhamResponseDto>();

            // Load dropdown riêng

            //load danh muc loai tui
            var loaiTuiList = await _danhMucLoaiTuiService.GetAllAsync();
            var modelLoaiTui = loaiTuiList.Data ?? new List<DanhMucLoaiTuiDto>();

            ViewBag.LoaiTuiList = modelLoaiTui
                .Select(x => new SelectListItem { Value = x.MaLoaiTui.ToString(), Text = x.TenLoaiTui })
                .ToList();

            //load thuong hieu
            var thuongHieuList = await _thuongHieuService.GetAllAsync();
            var modelThuongHieu = thuongHieuList.Data ?? new List<ThuongHieuDto>();
            ViewBag.ThuongHieuList = modelThuongHieu
                .Select(x => new SelectListItem { Value = x.MaThuongHieu.ToString(), Text = x.TenThuongHieu })
                .ToList();

            //load chat lieu
            var chatLieuList = await _chatLieuService.GetAllAsync();
            var modelChatLieu = chatLieuList.Data ?? new List<ChatLieuDto>();
            ViewBag.ChatLieuList = modelChatLieu
                .Select(x => new SelectListItem { Value = x.MaChatLieu.ToString(), Text = x.TenChatLieu })
                .ToList();

            return View(model);
        }

        // Hiển thị chi tiết sản phẩm với quản lý chi tiết và ảnh
        public async Task<IActionResult> Detail(int id)
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
    }
}