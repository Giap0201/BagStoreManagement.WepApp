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

        // Trang chủ: hiển thị top 10 sản phẩm mới nhất
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

            // Lấy top 10 sản phẩm mới nhất (sắp xếp theo NgayCapNhat giảm dần)
            var sanPhams = query
                .OrderByDescending(sp => sp.NgayCapNhat)
                .Take(10)
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