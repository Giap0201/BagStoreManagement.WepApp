using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Models.Entities;
using BagStore.Web.Repositories.implementations;
using BagStore.Web.Repositories.Implementations;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Implementations;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BagStore.Services.Interfaces;
using BagStore.Services.Implementations;
using BagStore.Repositories;
using BagStore.Services;
using BagStore.Web.AppConfig.Interface;
using BagStore.Web.AppConfig.Implementations;
using BagStore.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// ============================
// 1️⃣ Cấu hình DbContext
// ============================
builder.Services.AddDbContext<BagStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BagStoreDbContext")));

// ============================
// 2️⃣ Đăng ký Identity
// ============================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<BagStoreDbContext>()
.AddDefaultTokenProviders();

// ============================
// 3️⃣ Cấu hình JWT (API)
// ============================
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"];
var jwtIssuer = jwtSection["Issuer"];
var jwtAudience = jwtSection["Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
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

// ============================
// 4️⃣ CORS
// ============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ============================
// 5️⃣ Đăng ký Repositories & Services
// ============================
builder.Services.AddScoped<IUserService, UserService>();
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
builder.Services.AddScoped<IDonHangRepository, DonHangImpl>();
builder.Services.AddScoped<IChiTietDonHangRepository, ChiTietDonHangImpl>();
builder.Services.AddScoped<IDonHangService, DonHangService>();
builder.Services.AddScoped<IAnhSanPhamRepository, AnhSanPhamImpl>();
builder.Services.AddScoped<IAnhSanPhamService, AnhSanPhamService>();

// Đăng ký EnumMapper để chuyển đổi giữa chuỗi và enum
builder.Services.AddScoped<IEnumMapper, EnumMapper>();

// ============================
// 6️⃣ Controllers + Global Filter
// ============================
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ValidateModelAttribute>(); // tự động validate model trả BaseResponse
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// ============================
// 7️⃣ HttpClient
// ============================
builder.Services.AddHttpClient();

// ============================
// 8️⃣ Build app
// ============================
var app = builder.Build();

// ============================
// 9️⃣ Tạo role + admin mặc định nếu chưa có
// ============================
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roleNames = { "Admin", "Customer", "Employee" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

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
        var result = await userManager.CreateAsync(adminUser, "Admin@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// ============================
// 10️⃣ Middleware
// ============================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<ExceptionMiddleware>(); // Global exception
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// ============================
// 11️⃣ Map Controllers & Routes
// ============================
app.MapControllers();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ============================
// 12️⃣ Run
// ============================
app.Run();