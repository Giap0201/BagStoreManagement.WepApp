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
using BagStore.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using BagStore.Web.Services.Implementations;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ============================
// 1️⃣ Cấu hình DbContext
// ============================
builder.Services.AddDbContext<BagStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BagStoreDbContext")));

// ✅ Đăng ký Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<BagStoreDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.

//NguyenKhanhSon

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"];
var jwtIssuer = jwtSection["Issuer"];
var jwtAudience = jwtSection["Audience"];

//
// IMPORTANT: set default scheme to cookie (Identity) so MVC still uses cookies.
// Then add JwtBearer so API can explicitly require it.
//
builder.Services.AddAuthentication(options =>
{
    // Use cookie authentication as default for MVC
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
// keep Identity's cookies (AddIdentity already registered cookies) — just add JwtBearer
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});

// enable CORS for AJAX (adjust origins)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhostFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7013") // hoặc frontend origin(s)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddScoped<IUserService, UserService>();

//End NguyenKhanhSon
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


builder.Services.AddControllersWithViews();
//
builder.Services.AddHttpClient();

var app = builder.Build();

//NguyenKhanhSon

//Tự tạo role + admin nếu chưa có
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // 1️⃣ Tạo role ADMIN nếu chưa có
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    // 2️⃣ Tạo tài khoản admin mặc định
    var adminEmail = "admin@bagstore.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = "admin",
            FullName = "Administrator",
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Admin@123"); // ✅ password hash hợp lệ
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

//End NguyenKhanhSon

// Ví dụ trong Program.cs:
app.UseExceptionHandler(appBuilder =>
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



// Configure the HTTP request pipeline.
// ============================
// 7️⃣ Middleware cơ bản
// ============================
if (!app.Environment.IsDevelopment())
{
    app.UseHsts(); // bảo mật, chỉ chạy trong production
}

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage(); // ✅ Hiện lỗi chi tiết khi dev
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowLocalhostFrontend"); // dùng CORS policy nếu cần (NKS)

app.UseCors("AllowAll"); // thêm dòng này TRƯỚC UseAuthentication / UseAuthorization (NKS)

app.UseDeveloperExceptionPage();
app.UseAuthentication(); // Thêm dòng này để kích hoạt xác thực
app.UseAuthorization();

app.MapControllers(); // nếu bạn có API controller (NKS)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//NguyenKhanhSon

//End NguyenKhanhSon

app.Run();

