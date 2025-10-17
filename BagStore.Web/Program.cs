using BagStore.Data;
using BagStore.Web.Models.Entities;
using BagStore.Web.Repositories.implementations;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Add DbContext
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
builder.Services.AddScoped<IDanhMucLoaiTuiRepository, DanhMucLoaiTuiImpl>();
builder.Services.AddScoped<IThuongHieuRepository, ThuongHieuImpl>();
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
{
    appBuilder.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        // Log lỗi chi tiết tại đây (chỉ ghi log, không hiển thị ra ngoài)
        // logger.LogError(exception, "An unhandled exception occurred.");

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        // Trả về lỗi 500 chung và an toàn (Problem Details)
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Title = "Lỗi máy chủ nội bộ",
            Detail = "Đã xảy ra lỗi không mong muốn. Vui lòng thử lại sau.",
            Instance = context.Request.Path
        });
    });
});
//
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<BagStoreDbContext>();
//    // Áp dụng các migration còn thiếu (nếu có)
//    dbContext.Database.Migrate();
//}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
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

