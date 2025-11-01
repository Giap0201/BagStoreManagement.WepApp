using BagStore.Data;

using BagStore.Web.Models.Entities;
using BagStore.Web.Repositories.implementations;
using BagStore.Web.Repositories.Implementations;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Implementations;
using BagStore.Web.Services.Interfaces;
using BagStore.Web.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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
// 1️⃣ DbContext
// ============================
builder.Services.AddDbContext<BagStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BagStoreDbContext")));

// ============================
// 2️⃣ Identity
// ============================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<BagStoreDbContext>()
.AddDefaultTokenProviders();

// ⚙️ ✅ Cấu hình đường dẫn Login/AccessDenied mặc định
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Client/Account/Login";        // 👈 sửa chỗ này
    options.AccessDeniedPath = "/Client/Account/AccessDenied"; // tuỳ chọn
});

// ============================
// 3️⃣ JWT Authentication
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
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// ============================
// 5️⃣ Register Repositories & Services
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
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<IEnumMapper, EnumMapper>();

// ============================
// 6️⃣ Controllers
// ============================
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
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
// 8️⃣ Build App
// ============================
var app = builder.Build();

// ============================
// 9️⃣ Create default admin
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
// 🔟 Middleware
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

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
    RequestPath = "/uploads"
});

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// ============================
// 11️⃣ Map Routes
// ============================
app.MapControllers();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Client}/{controller=Home}/{action=Index}/{id?}");

// ✅ Optional: Debug route viewer (fix lỗi Body inferred)
app.MapGet("/routes", ([FromServices] IEnumerable<EndpointDataSource> sources) =>
{
    var routeNames = sources
        .SelectMany(ds => ds.Endpoints)
        .Select(e => e.DisplayName)
        .Where(n => !string.IsNullOrEmpty(n))
        .ToList();
    return Results.Ok(routeNames);
});

// ============================
// 12️⃣ Run
// ============================
app.Run();
