using BagStore.Web.Models.Entities;
using BagStore.Web.Models.ViewModels;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Area("Client")]
    public class AccountController : Controller
    {

        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpClientFactory _clientFactory;

        public AccountController(
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IHttpClientFactory clientFactory)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _clientFactory = clientFactory;
        }

        // GET: /Client/Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //JWT Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _clientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7013/api/auth/login", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                return View(model);
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseBody);
            var token = doc.RootElement.GetProperty("token").GetString();

            // ⚡ Lưu token tạm vào TempData để JS đọc
            TempData["JwtToken"] = token;

            // ✅ Chuyển hướng sang trang khách hàng
            return RedirectToAction("Index", "Customer", new { area = "Admin" });
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // GET: /Client/Account/Profile
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // lấy userId từ Claim của user hiện tại
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Challenge(); // hoặc RedirectToAction("Login");

            var user = await _userService.GetProfileAsync(userId);
            if (user == null) return NotFound();
            //return View(user);

            var model = new ProfileEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                NgaySinh = user.NgaySinh
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // optional: ensure the user edits only their own profile
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != model.Id)
                return Forbid();

            var result = await _userService.UpdateProfileAsync(model);
            if (result.Succeeded)
            {
                TempData["ProfileSuccess"] = "Cập nhật thông tin thành công";
                return RedirectToAction("Profile");
            }

            foreach (var err in result.Errors)
                ModelState.AddModelError("", err.Description);

            return View(model);
        }

        // POST: /Client/Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

    }
}
