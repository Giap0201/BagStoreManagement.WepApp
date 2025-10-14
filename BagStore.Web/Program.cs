using BagStore.Data;
using BagStore.Web.Models.Entities;
using BagStore.Web.Repositories.implementations;
using BagStore.Web.Repositories.Implementations;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Implementations;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// 1️⃣ Cấu hình DbContext
// ----------------------------
builder.Services.AddDbContext<BagStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BagStoreDbContext")));

// ----------------------------
// 2️⃣ Cấu hình Identity
// ----------------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<BagStoreDbContext>()
.AddDefaultTokenProviders();

// ----------------------------
// 3️⃣ Đăng ký Repositories (Scoped)
// ----------------------------
builder.Services.AddScoped<IDanhMucLoaiTuiRepository, DanhMucLoaiTuiImpl>();
builder.Services.AddScoped<IDanhMucLoaiTuiService, DanhMucLoaiTuiService>();
builder.Services.AddScoped<IThuongHieuRepository, ThuongHieuImpl>();
builder.Services.AddScoped<IThuongHieuService, ThuongHieuService>();
builder.Services.AddScoped<IChatLieuRepository, ChatLieuImpl>();
builder.Services.AddScoped<IChatLieuService, ChatLieuService>();
builder.Services.AddScoped<IMauSacRepository, MauSacImpl>();
builder.Services.AddScoped<IMauSacService, MauSacService>();
builder.Services.AddScoped<IKichThuocRepository, KichThuocImpl>();
builder.Services.AddScoped<IKichThuocService, KichThuocService>();
builder.Services.AddScoped<ISanPhamRepository, SanPhamImpl>();
builder.Services.AddScoped<ISanPhamService, SanPhamService>();
builder.Services.AddScoped<IChiTietSanPhamRepository, ChiTietSanPhamImpl>();
builder.Services.AddScoped<IChiTietSanPhamService, ChiTietSanPhamService>();

// ----------------------------
// 4️⃣ Cấu hình HttpClient để gọi API
// ----------------------------
//builder.Services.AddHttpClient("BagStoreApi", client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7013/");
//    client.DefaultRequestHeaders.Accept.Clear();
//    client.DefaultRequestHeaders.Accept.Add(
//        new MediaTypeWithQualityHeaderValue("application/json"));
//});

// ----------------------------
// 5️⃣ MVC
// ----------------------------
builder.Services.AddControllersWithViews();

// ----------------------------
// Build ứng dụng
// ----------------------------
var app = builder.Build();

// ----------------------------
// 6️⃣ Exception handler toàn cục
// ----------------------------
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        // Ghi log chi tiết ở đây (nếu muốn)
        // logger.LogError(exception, "Unhandled exception occurred");

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

// ----------------------------
// 7️⃣ Middleware cơ bản
// ----------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Cần cho Identity
app.UseAuthorization();

// ----------------------------
// 8️⃣ Route
// ----------------------------
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ----------------------------
// 9️⃣ Chạy ứng dụng
// ----------------------------
app.Run();