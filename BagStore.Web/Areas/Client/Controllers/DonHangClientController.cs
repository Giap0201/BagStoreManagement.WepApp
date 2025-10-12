using BagStore.Data;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Models.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Area("Client")]
    public class DonHangClientController : Controller
    {
        private readonly IDonHangRepository _donHangRepo;
        private readonly IChiTietDonHangRepository _chiTietRepo;
        private readonly BagStoreDbContext _context;
        private readonly UserManager<KhachHang> _userManager;

        public DonHangClientController(
        IDonHangRepository donHangRepo,
        IChiTietDonHangRepository chiTietRepo,
        BagStoreDbContext context,
        UserManager<KhachHang> userManager)
        {
            _donHangRepo = donHangRepo;
            _chiTietRepo = chiTietRepo;
            _context = context;
            _userManager = userManager;
        }

        // GET: /Client/Orders
        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User); // lấy user hiện tại
            if (user == null)
                return RedirectToAction("Login", "Account");

            var maKh = user.MaKH;  // đây mới là MaKh chính xác
            var donHangs = await _donHangRepo.LayDonHangTheoKhachHangAsync(maKh);
            return View(donHangs);
        }

        // GET: /Client/Orders/Checkout
        public IActionResult Checkout()
        {
            return View();
        }

        // POST: /Client/Orders/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(DonHangTaoDTO dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập trước khi đặt hàng.";
                return RedirectToAction("Login", "Account");
            }

            dto.MaKH = user.MaKH;

            if (!ModelState.IsValid || dto.ChiTietDonHangs.Count == 0)
            {
                TempData["Error"] = "Dữ liệu đơn hàng không hợp lệ.";
                return View(dto);
            }

            // Tạo DonHang và cập nhật SoLuongTon trong transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var donHang = new BagStore.Domain.Entities.DonHang
                {
                    MaKH = dto.MaKH,
                    DiaChiGiaoHang = dto.DiaChiGiaoHang,
                    PhuongThucThanhToan = dto.PhuongThucThanhToan,
                    NgayDatHang = DateTime.Now,
                    TongTien = dto.ChiTietDonHangs.Sum(ct => ct.SoLuong * ct.GiaBan),
                    TrangThai = "Chờ xác nhận",
                    TrangThaiThanhToan = "Chưa thanh toán",
                    PhiGiaoHang = 0
                };

                await _donHangRepo.ThemAsync(donHang);
                await _donHangRepo.LuuAsync();

                foreach (var ct in dto.ChiTietDonHangs)
                {
                    var chiTietSanPham = await _context.ChiTietSanPhams
                        .FirstOrDefaultAsync(p => p.MaChiTietSP == ct.MaChiTietSP);

                    if (chiTietSanPham == null || chiTietSanPham.SoLuongTon < ct.SoLuong)
                    {
                        throw new Exception($"Sản phẩm {ct.MaChiTietSP} không đủ tồn kho.");
                    }

                    chiTietSanPham.SoLuongTon -= ct.SoLuong;
                    _context.ChiTietSanPhams.Update(chiTietSanPham);

                    var chiTiet = new BagStore.Domain.Entities.ChiTietDonHang
                    {
                        MaDonHang = donHang.MaDonHang,
                        MaChiTietSP = ct.MaChiTietSP,
                        SoLuong = ct.SoLuong,
                        GiaBan = ct.GiaBan
                    };
                    await _chiTietRepo.ThemAsync(chiTiet);
                }

                await _chiTietRepo.LuuAsync();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "Đặt hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = ex.Message;
                return View(dto);
            }
        }
    }
}
