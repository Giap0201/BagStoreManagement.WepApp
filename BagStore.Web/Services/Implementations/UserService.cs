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
using Microsoft.Extensions.Configuration;

namespace BagStore.Web.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly BagStoreDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            BagStoreDbContext dbContext,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _configuration = configuration;
        }


        // ========================== CUSTOMER REGISTER ==========================
        public async Task<IdentityResult> RegisterCustomerAsync(RegisterViewModel model)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FullName = model.FullName,
                    NgaySinh = model.NgaySinh
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return result;

                await _userManager.AddToRoleAsync(user, "CUSTOMER");

                // 👉 tạo khách hàng tương ứng
                var kh = new KhachHang
                {
                    TenKH = user.FullName,
                    SoDienThoai = user.PhoneNumber,
                    DiaChiMacDinh = "Chưa cập nhật",  // ✅ thêm dòng này
                    NgayDangKy = DateTime.Now,
                    ApplicationUserId = user.Id
                };

                _dbContext.KhachHangs.Add(kh);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                // Lấy thông tin lỗi chi tiết nhất (inner nhất)
                var inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;

                throw new Exception($"Lỗi khi thêm khách hàng: {inner.Message}", inner);
            }
        }


        public async Task<SignInResult> LoginAsync(LoginViewModel model)
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

        public async Task<ApplicationUser?> GetProfileAsync(string userId)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IdentityResult> UpdateProfileAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateProfileAsync(ProfileEditViewModel model)
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

            if (!result.Succeeded)
                return result;

            // 3️⃣ Đồng bộ sang bảng KhachHang (nếu có)
            var kh = await _dbContext.KhachHangs
                .FirstOrDefaultAsync(k => k.ApplicationUserId == user.Id);

            if (kh != null)
            {
                kh.TenKH = user.FullName ?? user.UserName;
                kh.SoDienThoai = user.PhoneNumber ?? "";
                //kh.DiaChiMacDinh = model.DiaChiMacDinh ?? kh.DiaChiMacDinh; // nếu form có địa chỉ thì cập nhật
                _dbContext.KhachHangs.Update(kh);
                await _dbContext.SaveChangesAsync();
            }

            return result;
        }

        //public async Task<IdentityResult> DeleteAccountAsync(string userId, string currentPassword)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //        return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy người dùng." });

        //    // 1) Kiểm tra mật khẩu
        //    var pwValid = await _userManager.CheckPasswordAsync(user, currentPassword);
        //    if (!pwValid)
        //        return IdentityResult.Failed(new IdentityError { Description = "Mật khẩu không đúng." });

        //    // 2) (TÙY) Xóa các dữ liệu liên quan thủ công nếu cần
        //    // Ví dụ: xóa KhachHang, NhanVienProfile, GioHang, DonHang,... 
        //    // Nếu bạn đã cấu hình cascade delete thì không cần đoạn này.
        //    try
        //    {
        //        // Ví dụ xóa KhachHang liên quan:
        //        var kh = await _dbContext.KhachHangs.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id);
        //        if (kh != null)
        //        {
        //            _dbContext.KhachHangs.Remove(kh);
        //        }

        //        var nv = await _dbContext.NhanVienProfiles.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id);
        //        if (nv != null)
        //        {
        //            _dbContext.NhanVienProfiles.Remove(nv);
        //        }

        //        // TODO: xóa các bảng khác tuỳ schema của bạn (GioHang, DonHang, ...)
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    catch
        //    {
        //        // nếu có lỗi xóa quan hệ, bạn có thể log nhưng tiếp tục xóa user hoặc trả về lỗi
        //    }

        //    // 3) Xóa user (Identity)
        //    var result = await _userManager.DeleteAsync(user);

        //    // 4) Nếu xóa user thành công, sign out (clear cookie)
        //    if (result.Succeeded)
        //    {
        //        await _signInManager.SignOutAsync();
        //    }

        //    return result;
        //}

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Người dùng không tồn tại" });

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result;
        }


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

            var result = await _userManager.CreateAsync(user, model.Password ?? "Customer@123");
            if (!result.Succeeded)
                return result;

            await _userManager.AddToRoleAsync(user, "Customer");

            // ➕ Tạo khách hàng tương ứng
            var kh = new KhachHang
            {
                TenKH = model.FullName,
                SoDienThoai = model.PhoneNumber,
                DiaChiMacDinh = "Chưa cập nhật",  // ✅ thêm dòng này
                NgayDangKy = DateTime.Now,
                ApplicationUserId = user.Id
            };

            _dbContext.KhachHangs.Add(kh);
            await _dbContext.SaveChangesAsync();

            return result;
        }


        public async Task<IdentityResult> UpdateCustomerAsync(AdminCustomerViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy người dùng" });

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.NgaySinh = model.NgaySinh;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return result;

            // ➕ Đồng bộ thông tin KhachHang
            var kh = await _dbContext.KhachHangs.FirstOrDefaultAsync(k => k.ApplicationUserId == user.Id);
            if (kh != null)
            {
                kh.TenKH = model.FullName;
                kh.SoDienThoai = model.PhoneNumber;
                await _dbContext.SaveChangesAsync();
            }

            return result;
        }


        public async Task<IdentityResult> DeleteAccountAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy người dùng" });

            // ➖ Khi xoá user, KhachHang sẽ tự xoá nhờ OnDelete(DeleteBehavior.Cascade)
            return await _userManager.DeleteAsync(user);
        }


        public async Task<IdentityResult> ResetPasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy người dùng" });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<ApplicationUser?> GetProfileByUserNameAsync(string username)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }


        //JWT Token
        private string CreateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection["Key"];
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];
            var expiresMinutes = int.Parse(jwtSection["ExpiresMinutes"] ?? "120");

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
    };

            // add role claims
            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string?> GenerateJwtForUserAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return null;

            var valid = await _userManager.CheckPasswordAsync(user, password);
            if (!valid) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return CreateJwtToken(user, roles);
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? "")
        };

            // Thêm role (nếu có)
            var roles = _userManager.GetRolesAsync(user).Result;
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
