// Trong thư mục Data/ApplicationDbContext.cs

using BagStore.Domain.Entities;
using BagStore.Domain.Entities.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // --- Định nghĩa các DbSet cho tất cả các Entities của bạn ---

    // Identity & Profiles (ApplicationUser đã được xử lý bởi IdentityDbContext)
    public DbSet<KhachHangProfile> KhachHangProfiles { get; set; } = null!;

    public DbSet<NhanVienProfile> NhanVienProfiles { get; set; } = null!;

    // Danh Mục
    public DbSet<DanhMucLoaiTui> DanhMucLoaiTuis { get; set; } = null!;

    public DbSet<ThuongHieu> ThuongHieus { get; set; } = null!;
    public DbSet<ChatLieu> ChatLieus { get; set; } = null!;
    public DbSet<MauSac> MauSacs { get; set; } = null!;
    public DbSet<KichThuoc> KichThuocs { get; set; } = null!;

    // Sản Phẩm & Liên Quan
    public DbSet<SanPham> SanPhams { get; set; } = null!;

    public DbSet<ChiTietSanPham> ChiTietSanPhams { get; set; } = null!;
    public DbSet<AnhSanPham> AnhSanPhams { get; set; } = null!;
    public DbSet<GioHang> GioHangs { get; set; } = null!;
    public DbSet<DanhGia> DanhGias { get; set; } = null!;

    // Đơn Hàng & Thanh Toán
    public DbSet<DonHang> DonHangs { get; set; } = null!;

    public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; } = null!;
    public DbSet<ThanhToan> ThanhToans { get; set; } = null!;

    // Quản Lý Kho
    public DbSet<NhaCungCap> NhaCungCaps { get; set; } = null!;

    public DbSet<KhoHang> KhoHangs { get; set; } = null!;
    public DbSet<PhieuNhapHang> PhieuNhapHangs { get; set; } = null!;
    public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = null!;
    public DbSet<TonKho> TonKhos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // RẤT QUAN TRỌNG: Gọi phương thức OnModelCreating của lớp cơ sở (IdentityDbContext)
        // để cấu hình các bảng Identity (User, Role, UserClaims, etc.)
        base.OnModelCreating(modelBuilder);

        // ÁP DỤNG TẤT CẢ CÁC LỚP CẤU HÌNH IEntityTypeConfiguration<T>
        // có trong cùng Assembly với ApplicationDbContext.
        // Điều này sẽ áp dụng các cấu hình cho ApplicationUserConfig, KhachHangProfileConfig,
        // NhanVienProfileConfig và tất cả các Entities khác mà bạn đã tạo Configuration riêng.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Bạn có thể thêm dữ liệu ban đầu (seeding) ở đây nếu muốn.
        // Ví dụ: SeedDefaultRoles(modelBuilder);
        //         SeedAdminUser(modelBuilder);
    }

    // // Ví dụ về phương thức Seed Roles (có thể di chuyển ra một lớp riêng nếu muốn)
    // private void SeedDefaultRoles(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<IdentityRole>().HasData(
    //         new IdentityRole { Id = "admin-role-id", Name = "Admin", NormalizedName = "ADMIN" },
    //         new IdentityRole { Id = "customer-role-id", Name = "Customer", NormalizedName = "CUSTOMER" },
    //         new IdentityRole { Id = "employee-role-id", Name = "Employee", NormalizedName = "EMPLOYEE" }
    //     );
    // }
}