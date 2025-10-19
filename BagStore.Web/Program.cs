using BagStore.Data;
using BagStore.Domain.Entities;
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
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==============================
// 🔹 Đăng ký DbContext (SQL Server)
// ==============================
builder.Services.AddDbContext<BagStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BagStoreDbContext")));

// ==============================
// 🔹 Cấu hình Identity (ApplicationUser)
// ==============================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
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

// ==============================
// 🔹 Đăng ký Repository (DI Container)
// ==============================
// Bạn chỉ chịu trách nhiệm phần Đơn hàng => giữ lại phần liên quan
builder.Services.AddScoped<IDonHangRepository, DonHangImpl>();
builder.Services.AddScoped<IChiTietDonHangRepository, ChiTietDonHangImpl>();
builder.Services.AddScoped<IDonHangService, DonHangService>();

// Phần khác do team khác phụ trách — chỉ giữ lại nếu cần dùng chung
//builder.Services.AddScoped<ISanPhamRepository, SanPhamImpl>();
//builder.Services.AddScoped<IKhachHangRepository, KhachHangImpl>();
// ============================
// 3️⃣ Đăng ký Repositories & Services
// ============================
// Scoped: mỗi request tạo 1 instance
builder.Services.AddScoped<IDanhMucLoaiTuiRepository, DanhMucLoaiTuiImpl>();
builder.Services.AddScoped<IDanhMucLoaiTuiService, DanhMucLoaiTuiService>();
builder.Services.AddScoped<IThuongHieuRepository, ThuongHieuImpl>();
builder.Services.AddScoped<IThuongHieuService, ThuongHieuService>();
builder.Services.AddScoped<IChatLieuRepository, ChatLieuImpl>();
builder.Services.AddScoped<IDanhMucLoaiTuiRepository, DanhMucLoaiTuiImpl>();

// ==============================
// 🔹 Cấu hình MVC + HttpClient
// ==============================
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// ==============================
// 🔹 Xây dựng ứng dụng
// ==============================
var app = builder.Build();

// ==============================
// 🔹 Xử lý lỗi toàn cục (Global Exception Handler)
// ==============================
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        // ❗ Có thể ghi log chi tiết ở đây (Serilog, NLog, ...)
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

        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Title = "Lỗi máy chủ nội bộ",
            Detail = "Đã xảy ra lỗi không mong muốn. Vui lòng thử lại sau.",
            Instance = context.Request.Path
        });
    });
});

// ==============================
// 🔹 Cấu hình môi trường & middleware
// ==============================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
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

app.UseAuthentication(); // ⚠️ Cần có vì dùng Identity
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    // Bỏ qua xác thực cho tất cả request
    context.User = new System.Security.Claims.ClaimsPrincipal();
    await next.Invoke();
});

// ==============================
// 🔹 Định tuyến cho Areas (Admin / Client)
// ==============================
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// 🔹 Định tuyến mặc định (Home)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
