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
        public IActionResult Index()
        {
            // Lấy 8 sản phẩm mới nhất, có ảnh chính
            var sanPhams = _context.SanPhams
                .Include(sp => sp.AnhSanPhams)
                .Include(sp => sp.ThuongHieu)
                .Include(sp => sp.ChatLieu)
                .OrderByDescending(sp => sp.NgayCapNhat)
                .Take(8)
                .Select(sp => new
                {
                    sp.MaSP,
                    sp.TenSP,
                    sp.MoTaChiTiet,
                    sp.ThuongHieu.TenThuongHieu,
                    sp.ChatLieu.TenChatLieu,
                    AnhChinh = sp.AnhSanPhams.FirstOrDefault(a => a.LaHinhChinh).DuongDan
                })
                .ToList();

            return View(sanPhams);
        }

        public IActionResult TestView()
        {
            return View();
        }
    }
}