using BagStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Area("Client")]
    public class HomeController : Controller
    {
        private readonly BagStoreDbContext _context;

        public HomeController(BagStoreDbContext context)
        {
            _context = context;
        }

        // Trang chủ: hiển thị danh sách sản phẩm mới
        public IActionResult Index(int? loaiTuiId)
        {
            // Query base
            var query = _context.SanPhams
                .Include(sp => sp.AnhSanPhams)
                .Include(sp => sp.ThuongHieu)
                .Include(sp => sp.ChatLieu)
                .AsQueryable();

            // Lọc theo loại túi nếu có
            if (loaiTuiId.HasValue)
            {
                query = query.Where(sp => sp.MaLoaiTui == loaiTuiId.Value);
            }

            // Lấy sản phẩm (nếu có filter thì lấy tất cả, không thì lấy 8 sản phẩm mới nhất)
            var sanPhams = query
                .OrderByDescending(sp => sp.NgayCapNhat)
                .Take(loaiTuiId.HasValue ? 100 : 8) // Nếu có filter, lấy nhiều hơn
                .Select(sp => new
                {
                    sp.MaSP,
                    sp.TenSP,
                    sp.MoTaChiTiet,
                    TenThuongHieu = sp.ThuongHieu != null ? sp.ThuongHieu.TenThuongHieu : "",
                    TenChatLieu = sp.ChatLieu != null ? sp.ChatLieu.TenChatLieu : "",
                    AnhChinh = sp.AnhSanPhams.FirstOrDefault(a => a.LaHinhChinh) != null 
                        ? sp.AnhSanPhams.FirstOrDefault(a => a.LaHinhChinh).DuongDan 
                        : ""
                })
                .ToList();

            // Pass filter info to view
            ViewBag.LoaiTuiId = loaiTuiId;
            ViewBag.HasFilter = loaiTuiId.HasValue;

            return View(sanPhams);
        }

        public IActionResult TestView()
        {
            return View();
        }
    }
}