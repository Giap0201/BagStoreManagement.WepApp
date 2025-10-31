using BagStore.Domain.Entities;
using BagStore.Services.Interfaces;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Models.ViewModels;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        private readonly IMauSacService _mauSacService;
        private readonly IKichThuocService _kichThuocService;

        public SanPhamController(
         ISanPhamService service,
         IThuongHieuService thuongHieuService,
         IChatLieuService chatLieuService,
         IDanhMucLoaiTuiService danhMucLoaiTuiService,
         IChiTietSanPhamService chiTietService,
         IAnhSanPhamService anhService,
         IMauSacService mauSacService,
         IKichThuocService kichThuocService)
        {
            _service = service;
            _chatLieuService = chatLieuService;
            _thuongHieuService = thuongHieuService;
            _danhMucLoaiTuiService = danhMucLoaiTuiService;
            _chiTietService = chiTietService;
            _anhService = anhService;
            _mauSacService = mauSacService;
            _kichThuocService = kichThuocService;
        }

        // Hiển thị danh sách sản phẩm (ban đầu)
        [Route("Admin/SanPham")]
        [Route("Admin/SanPham/Index")]
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
            //load mau sac
            var mauSacList = await _mauSacService.GetAllAsync();
            var modelMauSac = mauSacList.Data ?? new List<MauSacDto>();
            ViewBag.MauSacList = modelMauSac
                .Select(x => new SelectListItem { Value = x.MaMauSac.ToString(), Text = x.TenMauSac })
                .ToList();

            //load kich thuoc
            var kichThuocList = await _kichThuocService.GetAllAsync();
            var modelKichThuoc = kichThuocList.Data ?? new List<KichThuocDto>();
            ViewBag.KichThuocList = modelKichThuoc
                .Select(x => new SelectListItem { Value = x.MaKichThuoc.ToString(), Text = x.TenKichThuoc })
                .ToList();

            return View(model);
        }
    }
}