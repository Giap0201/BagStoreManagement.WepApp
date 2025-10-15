using BagStore.Data;
using BagStore.Web.Models.Entities;
using BagStore.Web.Repositories.implementations;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

// Tự tạo role + admin nếu chưa có
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

app.UseDeveloperExceptionPage();
app.UseAuthentication(); // Thêm dòng này để kích hoạt xác thực
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//NguyenKhanhSon

//End NguyenKhanhSon

app.Run();

