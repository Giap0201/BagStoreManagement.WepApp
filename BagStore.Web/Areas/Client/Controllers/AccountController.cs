using BagStore.Web.Models.ViewModels;
using BagStore.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Claims;
using BagStore.Web.Models.Entities;

namespace BagStore.Web.Areas.Client.Controllers
{
    [Area("Client")]
    public class AccountController : Controller
    {

        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Client/Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Client/Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //var result = await _userService.LoginAsync(model);
            //if (result.Succeeded)
            //    return RedirectToAction("Index", "Home", new { area = "Client" });

            var result = await _userService.LoginAsync(model);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Customer", new { area = "Admin" });
                    // nếu bạn chưa có DashboardController thì đổi thành CustomerController hoặc trang nào của admin
                }

                return RedirectToAction("Index", "Home", new { area = "Client" });
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            return View(model);
        }

        // GET: /Client/Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Client/Account/Register
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    var result = await _userService.RegisterAsync(model);
        //    if (result.Succeeded)
        //        return RedirectToAction("Login");

        //    foreach (var error in result.Errors)
        //        ModelState.AddModelError("", error.Description);

        //    return View(model);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            Console.WriteLine(">>> Register POST received");

            if (!ModelState.IsValid)
            {
                Console.WriteLine(">>> ModelState invalid");

                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }

                return View(model);
            }

            try
            {
                var result = await _userService.RegisterAsync(model);
                Console.WriteLine(">>> RegisterAsync executed");

                if (result.Succeeded)
                {
                    Console.WriteLine(">>> Register succeeded");
                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> EXCEPTION: " + ex.Message);
                throw; // cho hiển thị ra lỗi thật
            }
        }

        // GET: /Client/Account/Profile
        //public async Task<IActionResult> Profile(string userId)
        //{
        //    var user = await _userService.GetProfileAsync(userId);
        //    if (user == null) return NotFound();
        //    return View(user);
        //}

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

            var model = new ProfileEditModel
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
        public async Task<IActionResult> Profile(ProfileEditModel model)
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

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                ViewBag.Message = "Password changed successfully!";
                return View();
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            var model = new DeleteAccountModel { Id = userId };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteAccountModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != model.Id) return Forbid();

            var result = await _userService.DeleteAccountAsync(model.Id, model.CurrentPassword);
            if (result.Succeeded)
            {
                // Sau khi xóa xong, redirect tới trang chính hoặc thông báo
                return RedirectToAction("Index", "Home", new { area = "" }); // về trang public
            }

            foreach (var err in result.Errors)
                ModelState.AddModelError("", err.Description);

            return View(model);
        }

    }
}
