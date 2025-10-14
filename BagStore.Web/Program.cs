using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Models.Entities;
using BagStore.Web.Repositories.implementations;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Implementations;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddScoped<IThuongHieuRepository, ThuongHieuImpl>();
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

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

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
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // ⚠️ Cần có vì dùng Identity
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
