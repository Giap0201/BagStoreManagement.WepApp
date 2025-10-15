using BagStore.Data;
using BagStore.Web.Models;
using BagStore.Web.Models.Entities;
using BagStore.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Web.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly BagStoreDbContext _dbContext;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            BagStoreDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        //private readonly IConfiguration _configuration;

        //public UserService(UserManager<ApplicationUser> userManager,
        //                   SignInManager<ApplicationUser> signInManager,
        //                   IConfiguration configuration)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _configuration = configuration;
        //}

        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            //if (result.Succeeded)
            //    await _userManager.AddToRoleAsync(user, "Customer");

            if (result.Succeeded)
            {
                // optional: seed role check elsewhere; guard in case role doesn't exist
                if (await _userManager.IsInRoleAsync(user, "Customer") == false)
                {
                    try { await _userManager.AddToRoleAsync(user, "Customer"); } catch { /* ignore role errors for now */ }
                }
            }

            return result;
        }

        //public async Task<SignInResult> LoginAsync(LoginModel model)
        //{
        //    return await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
        //}

        public async Task<SignInResult> LoginAsync(LoginModel model)
        {
            // dùng PasswordSignInAsync cho cookie authentication
            var result = await _signInManager.PasswordSignInAsync(
                model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        //public async Task<ApplicationUser?> GetProfileAsync(string userId)
        //{
        //    return await _userManager.FindByIdAsync(userId);
        //}

        public async Task<ApplicationUser?> GetProfileAsync(string userId)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        //public async Task<IdentityResult> UpdateProfileAsync(ApplicationUser user)
        //{
        //    return await _userManager.UpdateAsync(user);
        //}

        public async Task<IdentityResult> UpdateProfileAsync(ProfileEditModel model)
        {
            // Lấy user hiện tại
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy người dùng" });

            // Nếu muốn cho phép đổi username/email, cần dùng UserManager.SetUserNameAsync / SetEmailAsync
            // Set property trực tiếp rồi UpdateAsync cũng được cho các trường bình thường
            user.UserName = model.UserName;
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.NgaySinh = model.NgaySinh;

            // Nếu đổi Email, bạn có thể cần cập nhật NormalizedEmail, hoặc gọi SetEmailAsync.
            // Lưu đơn giản:
            var result = await _userManager.UpdateAsync(user);

            // Nếu thay đổi email and you want to re-confirm, you might need to manage EmailConfirmed flag & tokens.
            return result;
        }

        public async Task<IdentityResult> DeleteAccountAsync(string userId, string currentPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy người dùng." });

            // 1) Kiểm tra mật khẩu
            var pwValid = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!pwValid)
                return IdentityResult.Failed(new IdentityError { Description = "Mật khẩu không đúng." });

            // 2) (TÙY) Xóa các dữ liệu liên quan thủ công nếu cần
            // Ví dụ: xóa KhachHang, NhanVienProfile, GioHang, DonHang,... 
            // Nếu bạn đã cấu hình cascade delete thì không cần đoạn này.
            try
            {
                // Ví dụ xóa KhachHang liên quan:
                var kh = await _dbContext.KhachHangs.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id);
                if (kh != null)
                {
                    _dbContext.KhachHangs.Remove(kh);
                }

                var nv = await _dbContext.NhanVienProfiles.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id);
                if (nv != null)
                {
                    _dbContext.NhanVienProfiles.Remove(nv);
                }

                // TODO: xóa các bảng khác tuỳ schema của bạn (GioHang, DonHang, ...)
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                // nếu có lỗi xóa quan hệ, bạn có thể log nhưng tiếp tục xóa user hoặc trả về lỗi
            }

            // 3) Xóa user (Identity)
            var result = await _userManager.DeleteAsync(user);

            // 4) Nếu xóa user thành công, sign out (clear cookie)
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
            }

            return result;
        }

        //    public string GenerateJwtToken(ApplicationUser user)
        //    {
        //        var claims = new[]
        //        {
        //    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //    new Claim(ClaimTypes.NameIdentifier, user.Id)
        //};

        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //        var token = new JwtSecurityToken(
        //            issuer: _configuration["Jwt:Issuer"],
        //            audience: _configuration["Jwt:Audience"],
        //            claims: claims,
        //            expires: DateTime.Now.AddHours(1),
        //            signingCredentials: creds
        //        );

        //        return new JwtSecurityTokenHandler().WriteToken(token);
        //    }


        //ADMIN

        public async Task<List<ApplicationUser>> GetAllCustomersAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("Customer");
            return users.ToList();
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> CreateCustomerAsync(AdminCustomerViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                NgaySinh = model.NgaySinh
            };

            var result = await _userManager.CreateAsync(user, model.Password!);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, "Customer");

            return result;
        }

        public async Task<IdentityResult> UpdateCustomerAsync(AdminCustomerViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy người dùng" });

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.NgaySinh = model.NgaySinh;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAccountAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy người dùng" });

            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy người dùng" });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }
    }
}
