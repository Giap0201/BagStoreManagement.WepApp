using BagStore.Data;
using BagStore.Repositories;
using BagStore.Services;
using BagStore.Models.Common;
using BagStore.Services.Implementations;
using BagStore.Services.Interfaces;
using BagStore.Web.Models.Entities;
using BagStore.Web.Repositories.implementations;
using BagStore.Web.Repositories.Implementations;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Implementations;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ============================
// 1️⃣ Cấu hình DbContext
// ============================
builder.Services.AddDbContext<BagStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BagStoreDbContext")));

// ============================
// 2️⃣ Cấu hình Identity
// ============================
// Quản lý user, role, authentication
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // không bắt xác nhận email
})
.AddEntityFrameworkStores<BagStoreDbContext>()
.AddDefaultTokenProviders();

// ============================
// 3️⃣ Đăng ký Repositories & Services
// ============================
// Scoped: mỗi request tạo 1 instance
builder.Services.AddScoped<IDanhMucLoaiTuiRepository, DanhMucLoaiTuiImpl>();
builder.Services.AddScoped<IDanhMucLoaiTuiService, DanhMucLoaiTuiService>();
builder.Services.AddScoped<IThuongHieuRepository, ThuongHieuImpl>();
builder.Services.AddScoped<IThuongHieuService, ThuongHieuService>();
builder.Services.AddScoped<IChatLieuRepository, ChatLieuImpl>();
builder.Services.AddScoped<IChatLieuService, ChatLieuService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IMauSacRepository, MauSacImpl>();
builder.Services.AddScoped<IMauSacService, MauSacService>();
builder.Services.AddScoped<IKichThuocRepository, KichThuocImpl>();
builder.Services.AddScoped<IKichThuocService, KichThuocService>();
builder.Services.AddScoped<ISanPhamRepository, SanPhamImpl>();
builder.Services.AddScoped<ISanPhamService, SanPhamService>();
builder.Services.AddScoped<IChiTietSanPhamRepository, ChiTietSanPhamImpl>();
builder.Services.AddScoped<IChiTietSanPhamService, ChiTietSanPhamService>();

// ============================
// 4️⃣ Add Controllers với View + Global Filter ValidateModel
// ============================
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ValidateModelAttribute>(); // vẫn giữ
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true; // ✅ quan trọng
});

// ============================
// 5️⃣ Build app
// ============================
var app = builder.Build();

// ============================
// 6️⃣ Middleware xử lý lỗi toàn cục
// ============================
// Bắt tất cả lỗi runtime chưa handle và trả BaseResponse chuẩn
app.UseMiddleware<ExceptionMiddleware>();

// ============================
// 7️⃣ Middleware cơ bản
// ============================
if (!app.Environment.IsDevelopment())
{
    app.UseHsts(); // bảo mật, chỉ chạy trong production
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();