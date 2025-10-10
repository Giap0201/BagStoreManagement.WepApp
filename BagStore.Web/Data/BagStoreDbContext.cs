using BagStore.Data.Configurations;
using BagStore.Domain.Entities;
using BagStore.Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Data
{
    // BagStoreDbContext kế thừa IdentityDbContext để tích hợp Identity
    // ApplicationUser là user tùy chỉnh của hệ thống, quản lý Admin và Client
    public class BagStoreDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public BagStoreDbContext(DbContextOptions<BagStoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<DanhMucLoaiTui> DanhMucLoaiTuis { get; set; }
        public DbSet<ThuongHieu> ThuongHieux { get; set; }
        public DbSet<ChatLieu> ChatLieus { get; set; }
        public DbSet<MauSac> MauSacs { get; set; }
        public DbSet<KichThuoc> KichThuocs { get; set; }

        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<ChiTietSanPham> ChiTietSanPhams { get; set; }
        public DbSet<AnhSanPham> AnhSanPhams { get; set; }

        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVienProfile> NhanVienProfiles { get; set; }

        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<PhieuNhapHang> PhieuNhapHangs { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

        public DbSet<DanhGia> DanhGias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // quan trọng để Identity hoạt động

            // Tự động áp dụng tất cả các cấu hình trong assembly này
            // Cách này giúp không phải khai báo từng config một
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BagStoreDbContext).Assembly);

            // ========================= Identity seed data ví dụ =========================
            // Tạo 1 admin mặc định
            // Password có thể hash bằng Identity khi seed hoặc để tạo sau khi chạy
            /*
            var adminId = "admin-id-guid";
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@bagstore.com",
                NormalizedEmail = "ADMIN@BAGSTORE.COM",
                EmailConfirmed = true,
                SecurityStamp = string.Empty,
                // PasswordHash có thể tạo bằng UserManager trước
            });
            */

            // ========================= Note =========================
            // - Nếu muốn phân quyền Admin/Client:
            //   + Tạo Role: Admin, Client
            //   + Gán Role cho ApplicationUser
            //   + Trong code, check User.IsInRole("Admin") hoặc "Client"
            // - KhachHang có thể dùng ApplicationUser.Id nếu muốn liên kết
            // - NhanVienProfile có thể dùng ApplicationUser.Id để quản lý nhân viên
        }
    }
}